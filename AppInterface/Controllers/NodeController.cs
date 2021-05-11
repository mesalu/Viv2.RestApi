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
    [Authorize(Policy = PolicyNames.UserAccess)]
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    public class NodeController : ControllerBase
    {
        private readonly IClaimIdentityCompat _claimsCompat;
        private readonly IGetNodeControllerUseCase _getUseCase;

        public NodeController(IClaimIdentityCompat claimsCompat,
            IGetNodeControllerUseCase getUseCase)
        {
            _claimsCompat = claimsCompat;
            _getUseCase = getUseCase;
        }

        /// <summary>
        /// Gets a collection of controllers for which there exists
        /// an environment that is visible (owned or shared) by the active user
        /// </summary>
        /// <returns></returns>
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
        [HttpGet("visible")]
        public async Task<IActionResult> GetVisibleNodesForUser()
        {
            return await Task.FromResult(BadRequest("Not yet implemented"));
        }

        /// <summary>
        /// Create a new controller entry and associates it to the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterController()
        {
            return await Task.FromResult(BadRequest("Not yet implemented"));
        }
    }
}