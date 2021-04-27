using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Core.Services;

namespace Viv2.API.AppInterface.Controllers
{
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    public class EnvironmentController : ControllerBase 
    {
        private readonly IRegisterEnvironmentUseCase _registerEnvironment;
        private readonly IGetEnvironmentsUseCase _getEnvironments;
        private readonly IClaimIdentityCompat _claimsCompat;

        public EnvironmentController(IRegisterEnvironmentUseCase registerEnvironment,
            IGetEnvironmentsUseCase getEnvironments,
            IClaimIdentityCompat claimsCompat)
        {
            _registerEnvironment = registerEnvironment;
            _getEnvironments = getEnvironments;
            _claimsCompat = claimsCompat;
        }

        /// <summary>
        /// Ensures that the specified environment exists and is registered to the current user.
        ///
        /// Currently permitted to any authenticated entity (user or daemon). Should be locked down
        /// to just user, with it occurring as part of the direct-connect-setup process when we get there.
        /// </summary>
        /// <param name="envGuid"></param>
        /// <returns></returns>
        [Authorize(Policy = PolicyNames.AnyAuthenticated)]
        [HttpPost("{envGuid}/register")]
        public async Task<IActionResult> RegisterEnvToUser(Guid envGuid)
        {
            var request = new RegisterEnvironmentRequest
            {
                Owner = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                MfgId = envGuid,
                CreateMode = RegisterEnvironmentRequest.Mode.Touch,
            };

            Console.WriteLine($"Found user: {request.Owner}");            

            var port = new BasicPresenter<NewEntityResponse<Guid>>();
            var success = await _registerEnvironment.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response.Id) : BadRequest(ModelState);
        }

        /// <summary>
        /// Acquires the list of environments for the authorized user, replying with
        /// all of the environments IDs.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet]
        public async Task<IActionResult> GetAllEnvsForUser()
        {
            var request = new DataAccessRequest<IList<IEnvironment>>
            {
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                Strategy = DataAccessRequest<IList<IEnvironment>>.AcquisitionStrategy.All
            };
            
            var port = new BasicPresenter<GenericDataResponse<IList<IEnvironment>>>();
            var success = await _getEnvironments.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response.Result.Select(e => e.Id).ToList()) : BadRequest();
        }
        
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnvInfo(Guid id)
        {
            // piggy back off the 'get all' use.
            var request = new DataAccessRequest<IList<IEnvironment>>
            {
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                Strategy = DataAccessRequest<IList<IEnvironment>>.AcquisitionStrategy.All
            };
            
            var port = new BasicPresenter<GenericDataResponse<IList<IEnvironment>>>();
            var success = await _getEnvironments.Handle(request, port);

            if (success)
            {
                var match = port.Response.Result.FirstOrDefault(e => e.Id == id);
                if (match != null) return new OkObjectResult(match);
            }

            return BadRequest();
        }
    }
}