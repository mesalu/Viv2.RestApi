using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.AppInterface.Controllers
{
    
    [Route(RoutingStrings.BaseController)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly IRefreshTokenExchangeUseCase _refreshTokenExchange;

        public AuthController(ILoginUseCase loginUseCase, IRefreshTokenExchangeUseCase refreshTokenExchangeUseCase)
        {
            _loginUseCase = loginUseCase;
            _refreshTokenExchange = refreshTokenExchangeUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(ModelState);

            BasicPresenter<LoginResponse> port = new BasicPresenter<LoginResponse>();
            Console.WriteLine($"Username: {request.Username}");
            var success = await _loginUseCase.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response) : BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string userId, string refreshToken)
        {
            TokenExchangeRequest request = new TokenExchangeRequest
            {
                EncodedRefreshToken = refreshToken, UserId = userId
            };

            // submit a request to Core:
            BasicPresenter<LoginResponse> port = new BasicPresenter<LoginResponse>();

            var success = await _refreshTokenExchange.Handle(request, port);
            return (success) ? new OkObjectResult(port.Response) : BadRequest();
        }
    }
}
