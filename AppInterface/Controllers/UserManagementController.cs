using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.AppInterface.Controllers
{
    [Route(RoutingStrings.AdminController)]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IAddUserUseCase _addUser;

        public UserManagementController(IAddUserUseCase addUserUseCase)
        {
            _addUser = addUserUseCase;
            Console.WriteLine("This controller is instanced.");
        }

        [Authorize(Roles = RoleValues.Admin)]
        [HttpPost("add")]
        public async Task<IActionResult> CreateNew([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(request.UserName)
                || string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Password)) return BadRequest(ModelState);

            BasicPresenter<NewUserResponse> port = new BasicPresenter<NewUserResponse>();
            var result = await _addUser.Handle(request, port);

            return (result) ? new OkObjectResult(port.Response.UserId) : BadRequest();
        }
        
        [Authorize(Roles = RoleValues.Admin)]
        [HttpPost("promote")]
        public async Task<IActionResult> BumpToAdmin([FromBody] Guid userGuid)
        {
            return await Task.FromResult<IActionResult>(BadRequest("Not yet implemented"));
        }
    }
}