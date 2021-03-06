using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.UseCases
{
    public class RefreshTokenExchangeUseCase : IRefreshTokenExchangeUseCase
    {
        private readonly IUserStore _store;
        private readonly ITokenMinter _minter;
        private readonly IClaimsComposer _claimsComposer;
        
        public RefreshTokenExchangeUseCase(IUserStore store, ITokenMinter minter, IClaimsComposer claimsComposer)
        {
            _store = store;
            _minter = minter;
            _claimsComposer = claimsComposer;
        }
        
        public async Task<bool> Handle(TokenExchangeRequest message, IOutboundPort<LoginResponse> outputPort)
        {
            // Verify user exists.
            var user = await _store.GetUserById(Guid.Parse(message.UserId));
            if (user == null) return false;
            
            await _store.LoadRefreshTokens(user);
            
            // Verify refresh token specified by message is associated to user.
            var persistedToken = user.RefreshTokens
                .FirstOrDefault(rt => String.Compare(rt.Token, message.EncodedRefreshToken, StringComparison.Ordinal) == 0);

            if (persistedToken == null) return false;
            
            var roles = await _store.GetRoles(user);
            var roleClaims = new List<Claim>();
            roleClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            
            // Mint a new access token based on the access capacity specified by the refresh token.
            string mintedAccessToken;
            if (persistedToken.AccessCapacity == AccessLevelValues.User)
                mintedAccessToken = _minter.Mint(_claimsComposer.ComposeIdentity(user, roleClaims), TokenType.UserAccess);
            else if (persistedToken.AccessCapacity == AccessLevelValues.Daemon)
                mintedAccessToken = _minter.Mint(_claimsComposer.ComposeIdentity(user), TokenType.DaemonAccess);
            else
            {
                // Malformed?
                return false;
            }
            
            // if a new refresh token is needed, mint a new one.
            var mintedRefreshToken = message.EncodedRefreshToken;
            
            // compose and submit response.
            var response = new LoginResponse
            {
                UserName = (persistedToken.AccessCapacity == AccessLevelValues.User) ? user.Name: null,
                AccessToken = new AccessToken
                {
                    Token = mintedAccessToken,
                    ExpiresIn = _minter.Options.TokenLifespan,
                },
                RefreshToken = new RefreshToken
                {
                    Token = mintedRefreshToken,
                    ExpiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(_minter.Options.RefreshTokenLifespan)),
                    IssuedTo = user.Guid,
                    IssuedBy = Dns.GetHostName()
                }
            };
            
            // TODO: if a new refresh token was issued, then push it to data store.
            
            outputPort.Handle(response);
            return true;
        }
    }
}
