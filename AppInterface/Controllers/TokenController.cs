﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.Services;

namespace Viv2.API.AppInterface.Controllers
{
    [Route(RoutingStrings.BaseController)]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IDataProviderGrantUseCase _dataProviderGrant;
        private readonly IRefreshTokenExchangeUseCase _refreshTokenExchange;
        private readonly IClaimIdentityCompat _claimCompat;
        
        public TokenController(IDataProviderGrantUseCase dataProviderGrant,
            IRefreshTokenExchangeUseCase refreshTokenExchange,
            IClaimIdentityCompat claimsCompat)
        {
            _dataProviderGrant = dataProviderGrant;
            _refreshTokenExchange = refreshTokenExchange;
            _claimCompat = claimsCompat;
        }

        /// <summary>
        /// For an authenticated user, generates and provides a token set for a daemon process that has
        /// received permission from the user to provide data on their behalf.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet("grant")]
        public async Task<IActionResult> AcquireTokenForDaemon()
        {
            BasicPresenter<LoginResponse> port = new BasicPresenter<LoginResponse>();
            ProviderGrantRequest request = new ProviderGrantRequest
            {
                OnBehalfOf = _claimCompat.ExtractFirstIdClaim(HttpContext.User) 
            };
            
            var success = await _dataProviderGrant.Handle(request, port);
            return (success) ?  new OkObjectResult(port.Response) : BadRequest();
        }

        /// <summary>
        /// Used for enacting a refresh token exchange.
        /// Can be used by either Client UIs representing an actual user or daemon bots.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="encodedToken"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshForDaemon([NotNull] string userId, [NotNull] string encodedToken)
        {
            TokenExchangeRequest request = new TokenExchangeRequest
            {
                EncodedRefreshToken = encodedToken, UserId = userId
            };

            // submit a request to Core:
            BasicPresenter<LoginResponse> port = new BasicPresenter<LoginResponse>();

            var success = await _refreshTokenExchange.Handle(request, port);
            return (success) ? new OkObjectResult(port.Response) : BadRequest();
        }
    }
}
