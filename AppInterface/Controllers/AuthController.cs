using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.AppInterface.Controllers
{
    
    [Route(RoutingStrings.BaseController)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;

        public AuthController(ILoginUseCase loginUseCase)
        {
            _loginUseCase = loginUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(ModelState);

            LoginAttemptPort port = new LoginAttemptPort();
            var success = await _loginUseCase.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response) : BadRequest();
        }
    }
}
