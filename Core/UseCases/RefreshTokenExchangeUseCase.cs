using System;
using System.Linq;
using System.Net;
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
            if (user == null) return false;
            
            await _backingStore.LoadRefreshTokens(user);
            
            // Verify refresh token specified by message is associated to user.
            var persistedToken = user.RefreshTokens
                .FirstOrDefault(rt => String.Compare(rt.Token, message.EncodedRefreshToken, StringComparison.Ordinal) == 0);

            if (persistedToken == null) return false;
            
            // Mint a new access token based on the access capacity specified by the refresh token.
            string mintedAccessToken;
            if (persistedToken.AccessCapacity == AccessLevelValues.User)
                mintedAccessToken = _minter.Mint(_claimsComposer.ComposeIdentity(user), TokenType.UserAccess);
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
