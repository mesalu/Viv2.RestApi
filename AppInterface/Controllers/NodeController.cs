using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Dto;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.AppInterface.Controllers
{
    /// <summary>
    /// An interface for accessing and manipulating NodeController data.
    /// This class name is reduced to just `node`, as NodeControllerController was... unideal.
    /// </summary>
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    public class NodeController : ControllerBase
    {
        private readonly IClaimIdentityCompat _claimsCompat;
        private readonly IGetNodeControllerUseCase _getUseCase;
        private readonly IUpdateNodeUseCase _updateNodeUseCase;

        public NodeController(IClaimIdentityCompat claimsCompat,
            IGetNodeControllerUseCase getUseCase,
            IUpdateNodeUseCase updateNodeUseCase)
        {
            _claimsCompat = claimsCompat;
            _getUseCase = getUseCase;
            _updateNodeUseCase = updateNodeUseCase;
        }

        /// <summary>
        /// Gets a collection of controllers for which there exists
        /// an environment that is visible (owned or shared) by the active user
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet("mine")]
        public async Task<IActionResult> GetOwnedNodesForUser()
        {
            var request = new DataAccessRequest<IController>
            {
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                Strategy = DataAccessRequest<IController>.AcquisitionStrategy.All
            };

            var port = new BasicPresenter<GenericDataResponse<IController>>();
            var success = await _getUseCase.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response.Result.Select(ControllerDto.From)) : BadRequest();
        }
        
        /// <summary>
        /// Gets a collection of controllers for which there exists
        /// an environment that is visible (owned or shared) by the active user
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet("visible")]
        public async Task<IActionResult> GetVisibleNodesForUser()
        {
            return await Task.FromResult(BadRequest("Not yet implemented"));
        }

        /// <summary>
        /// Create a new controller entry and associates it to the authenticated user.
        ///
        /// Used during controller set up after the user has granted access to the
        /// node controller. 
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterController(Guid controllerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var request = new NodeUpdateRequest
            {
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                ControllerId = controllerId,
                Operation = NodeUpdateRequest.Mode.Create
            };

            var port = new BasicPresenter<GenericDataResponse<IController>>();
            var result = await _updateNodeUseCase.Handle(request, port);

            return (result) ? new OkObjectResult(ControllerDto.From(port.Response.Result.First())) : BadRequest();
        }
        
        /// <summary>
        /// Updates the associations of listed environments to indicate that they're
        /// currently connected to this controller.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Authorize(Policy = PolicyNames.DaemonAccess)]
        [HttpPost("peers")]
        public async Task<IActionResult> SetPeers([FromBody] PeerUpdateForm form)
        {
            if (!ModelState.IsValid
                || form.ControllerId == Guid.Empty) return BadRequest(ModelState);

            var request = new NodeUpdateRequest
            {
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                ControllerId = form.ControllerId,
                PeerIds = form.PeerIds,
                Operation = NodeUpdateRequest.Mode.PeerUpdate
            };

            var port = new BasicPresenter<GenericDataResponse<IController>>();
            var result = await _updateNodeUseCase.Handle(request, port);
            
            return (result) ? new OkObjectResult(ControllerDto.From(port.Response.Result.First())) : BadRequest();
        }
    }
}