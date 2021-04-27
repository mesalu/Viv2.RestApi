using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Core.Services;

namespace Viv2.API.Core.UseCases
{
    /// <summary>
    /// Concrete implementation for logging users in in normal runtime contexts.
    /// </summary>
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUserBackingStore _userStore;
        private readonly ITokenMinter _minter;
        private readonly IClaimsComposer _claimsComposer;
        
        public LoginUseCase(IUserBackingStore userStore, ITokenMinter minter, IClaimsComposer claimsComposer)
        {
            _userStore = userStore;
            _minter = minter;
            _claimsComposer = claimsComposer;
        }
        
        public async Task<bool> Handle(LoginRequest message, IOutboundPort<LoginResponse> outputPort)
        {
            var user = await _userStore.GetUserByName(message.Username);
            if (user == null) return false;
            
            var roles = await _userStore.GetRoles(user);
            var roleClaims = new List<Claim>();
            roleClaims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
                
            ClaimsIdentity identity = _claimsComposer.ComposeIdentity(user, roleClaims);
            var response = new LoginResponse
            {
                AccessToken = new AccessToken
                {
                    Token = _minter.Mint(identity, TokenType.UserAccess),
                    ExpiresIn = _minter.Options.TokenLifespan,
                },
                RefreshToken = new RefreshToken
                {
                    Token = _minter.Mint(identity, TokenType.Refresh),
                    ExpiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(_minter.Options.RefreshTokenLifespan)),
                    IssuedTo = user.Guid,
                    IssuedBy = Dns.GetHostName(),
                    AccessCapacity = RoleValues.User
                }
            };
            
            // Push the new refresh token to data store
            user.RefreshTokens.Add(response.RefreshToken);
            await _userStore.UpdateUser(user);
                
            outputPort.Handle(response);
            return true;
        }
    }
}
