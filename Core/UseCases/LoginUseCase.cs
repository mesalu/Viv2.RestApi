using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Viv2.API.Core.Dto;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
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
            User user = await _userStore.GetUserByName(message.Username);
            if (user != null)
            {
                ClaimsIdentity identity = _claimsComposer.ComposeIdentity(user);
                LoginResponse response = new LoginResponse
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
                        IssuedTo = user.Id,
                        IssuedBy = Dns.GetHostName()
                    }
                };
                
                outputPort.Handle(response);
                return true;
            }
            return false;
        }
    }
}
