using System;
using System.Linq;
using System.Net;
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
    public class RefreshTokenExchangeUseCase : IRefreshTokenExchangeUseCase
    {
        private readonly IUserBackingStore _backingStore;
        private readonly ITokenMinter _minter;
        private readonly IClaimsComposer _claimsComposer;
        
        public RefreshTokenExchangeUseCase(IUserBackingStore backingStore, ITokenMinter minter, IClaimsComposer claimsComposer)
        {
            _backingStore = backingStore;
            _minter = minter;
            _claimsComposer = claimsComposer;
        }
        
        public async Task<bool> Handle(TokenExchangeRequest message, IOutboundPort<LoginResponse> outputPort)
        {
            // Verify user exists.
            var user = await _backingStore.GetUserById(Guid.Parse(message.UserId));
            
            // Verify refresh token specified by message is associated to user.
            bool associatedToUser = 
                user.RefreshTokens.Select(u => u.Token).Contains(message.EncodedRefreshToken);

            if (!associatedToUser) return false;
            
            // TODO: load claims out of old token, to distinguish prior claims? That way this can be used for all refreshes
            
            // Mint a new access token.
            var mintedAccessToken = _minter.Mint(
                _claimsComposer.ComposeIdentity(user),
                TokenType.UserAccess);
            
            // if a new refresh token is needed, mint a new one.
            var mintedRefreshToken = message.EncodedRefreshToken;
            
            
            // compose and submit response.
            var identity = _claimsComposer.ComposeIdentity(user);
            LoginResponse response = new LoginResponse
            {
                AccessToken = new AccessToken
                {
                    Token = mintedAccessToken,
                    ExpiresIn = _minter.Options.TokenLifespan,
                },
                RefreshToken = new RefreshToken
                {
                    Token = mintedRefreshToken,
                    ExpiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(_minter.Options.RefreshTokenLifespan)),
                    IssuedTo = user.Id,
                    IssuedBy = Dns.GetHostName()
                }
            };
            
            // TODO: if a new refresh token was issued, then push it to data store.
            
            outputPort.Handle(response);
            return true;
        }
    }
}
