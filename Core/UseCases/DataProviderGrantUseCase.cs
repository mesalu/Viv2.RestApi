using System;
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
    public class DataProviderGrantUseCase : IDataProviderGrantUseCase
    {
        private readonly IUserBackingStore _userStore;
        private readonly ITokenMinter _minter;
        private readonly IClaimsComposer _claimsComposer;

        public DataProviderGrantUseCase(IUserBackingStore backingStore, ITokenMinter minter, IClaimsComposer composer)
        {
            _userStore = backingStore;
            _minter = minter;
            _claimsComposer = composer;
        }
        
        public async Task<bool> Handle(ProviderGrantRequest message, IOutboundPort<LoginResponse> outputPort)
        {
            // Verify the user is valid
            var user = await _userStore.GetUserById(message.OnBehalfOf);
            if (user == null) return false;

            // TODO: verify - in some manner - that this action request is valid beyond 'has a real user'
            
            // Mint a data access token.
            var response = new LoginResponse
            {
                AccessToken = new AccessToken
                {
                    Token = _minter.Mint(_claimsComposer.ComposeIdentity(user), TokenType.DaemonAccess),
                    ExpiresIn = _minter.Options.TokenLifespan
                },
                RefreshToken = new RefreshToken
                {
                    Token = _minter.Mint(_claimsComposer.ComposeIdentity(user), TokenType.DaemonAccess),
                    ExpiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(_minter.Options.RefreshTokenLifespan)),
                    IssuedTo = user.Guid,
                    IssuedBy = Dns.GetHostName(),
                    AccessCapacity = AccessLevelValues.Daemon
                }
            };
            user.RefreshTokens.Add(response.RefreshToken);
            await _userStore.UpdateUser(user);
            
            // signal success
            outputPort.Handle(response);
            return true;
        }
    }
}
