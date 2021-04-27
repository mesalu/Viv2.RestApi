using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IModifyUserRolesUseCase _modifyUserRoles;

        public UserManagementController(IAddUserUseCase addUserUseCase, IModifyUserRolesUseCase modifyUserRoles)
        {
            _addUser = addUserUseCase;
            _modifyUserRoles = modifyUserRoles;
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

            return (result) ? new OkObjectResult(port.Response.Id) : BadRequest();
        }
        
        [Authorize(Roles = RoleValues.Admin)]
        [HttpPost("promote/{userGuid}")]
        public async Task<IActionResult> BumpToAdmin(Guid userGuid)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var request = new ModifyUserRolesRequest
            {
                UserId = userGuid,
                Mode = ModifyUserRolesRequest.ModificationMode.Add,
                DesiredRoles = new[] {RoleValues.Admin}
            };

            var success = await _modifyUserRoles.Handle(request, new BasicPresenter<BlankResponse>());
            return (success) ? Ok() : BadRequest();
        }

        [Authorize(Roles = RoleValues.Admin)]
        [HttpPost("freeze/{userGuid}")]
        public async Task<IActionResult> FreezeAccount(Guid userGuid)
        {
            return await Task.FromResult<IActionResult>(BadRequest("Not yet implemented"));
        }
    }
}