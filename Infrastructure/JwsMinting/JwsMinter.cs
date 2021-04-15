using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Viv2.API.Core.ConfigModel;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Services;

namespace Viv2.API.Infrastructure.JwsMinting
{
    /// <summary>
    /// Implementation of ITokenMinter using signed JWT standards
    ///
    /// Note: Contents of payload is effectively plaintext, do not include any vulnerable information
    ///       in the payload.
    /// Note: The signing key is regenerated at random per instance, this means that the service added should be
    ///       singleton, and that any signed token will be rendered invalid if the minter is re-instanced (e.g. after
    ///       a program restart).
    /// </summary>
    public class JwsMinter : ITokenMinter
    {
        private readonly SecurityKey _securityKey;
        private readonly SigningCredentials _signingCredentials;
        public JwsMinter(MinterOptions options)
        {
            Options = options;
            _securityKey = new SymmetricSecurityKey(_GenerateKey());
            _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        }

        public TokenValidationParameters ValidationParameters => new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireAudience = false,
            RequireSignedTokens = true,
            IssuerSigningKey = _securityKey,
            ValidIssuer = Options.Issuer
        };
        
        public string Mint(ClaimsIdentity identity, TokenType type)
        {
            if (type == TokenType.Refresh) return _MintRefreshToken();
            
            // There's probably a better method for this, but DateTime.ToBinary is kinda vague about the return value's
            // 'anchor' point.
            var utcNow = (long) Math.Round((DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds);
            List<Claim> claims = new List<Claim>(new[]
            {
                // Jwt UniqueID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // Jwt Issued-At
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString(), ClaimValueTypes.Integer64),

                // User Id
                identity.FindFirst(c => c.Type == ClaimNames.UserId)
            });

            // Add roles as per `type`:
            switch (type)
            {
                case TokenType.UserAccess:
                    claims.Add(new Claim(ClaimTypes.Role, RoleValues.User));
                    break;
                case TokenType.DaemonAccess:
                    claims.Add(new Claim(ClaimTypes.Role, RoleValues.Bot));
                    break;
            }

            // Build a JWT out of the claims.
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: Options.Issuer,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.FromSeconds(Options.RefreshTokenLifespan)),
                signingCredentials: _signingCredentials
            );
            
            // Encode token to base64, sign, and return it.
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public IEnumerable<Claim> ParseAccessToken(string minted)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(minted);
            return token.Claims;
        }

        public MinterOptions Options { get; }

        /// <summary>
        /// Should be called precisely once at construction, generates a signing key to use
        /// for this processes' lifetime. 
        /// 
        /// Should the process be restarted for any reason, any old data signed by this key will be
        /// invalidated.
        /// </summary>
        /// <returns>A cryptographically random 128bit key</returns>
        private static byte[] _GenerateKey()
        {
            byte[] key = new byte[128];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(key);
            return key;
        }

        /// <summary>
        /// Generates a random loooong byte string and encodes it to base64, for use as a refresh token's payload.
        /// </summary>
        /// <returns></returns>
        private string _MintRefreshToken()
        {
            byte[] tokenBytes = new byte[512];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(tokenBytes);
            
            // encode to base64 and return.
            return Convert.ToBase64String(tokenBytes);
        }
    }
}
