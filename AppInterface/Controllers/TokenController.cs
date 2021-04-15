using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.AppInterface.Controllers
{
    [Route(RoutingStrings.BaseController)]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IDataProviderGrantUseCase _dataProviderGrant;
        private readonly IRefreshTokenExchangeUseCase _refreshTokenExchange;
        
        public TokenController(IDataProviderGrantUseCase dataProviderGrant,
            IRefreshTokenExchangeUseCase refreshTokenExchange)
        {
            _dataProviderGrant = dataProviderGrant;
            _refreshTokenExchange = refreshTokenExchange;
        }

        /// <summary>
        /// For an authenticated user, generates and provides a token set for a daemon process that has
        /// recieved permission from the user to provide data on their behalf.
        /// </summary>
        /// <returns></returns>
        [HttpPost("grant")]
        [Authorize(Roles = RoleValues.User)]
        public async Task<IActionResult> AcquireTokenForDaemon()
        {
            LoginAttemptPort port = new LoginAttemptPort();
            ProviderGrantRequest request = new ProviderGrantRequest
            {
                OnBehalfOf = Helpers.UserCompatHelper.UserGuidFromAuthenticatedContext(HttpContext)
            };
            
            var success = await _dataProviderGrant.Handle(request, port);
            return (success) ?  new OkObjectResult(port.Response) : BadRequest();
        }

        [HttpPost("daemon-refresh")]
        [Authorize(Roles = RoleValues.Bot)]
        public async Task<IActionResult> RefreshForDaemon([NotNull] string userId, [NotNull] string encodedToken)
        {
            TokenExchangeRequest request = new TokenExchangeRequest
            {
                EncodedRefreshToken = encodedToken, UserId = userId
            };

            // submit a request to Core:
            LoginAttemptPort port = new LoginAttemptPort();

            var success = await _refreshTokenExchange.Handle(request, port);
            return (success) ? new OkObjectResult(port.Response) : BadRequest();
        }
    }
}
