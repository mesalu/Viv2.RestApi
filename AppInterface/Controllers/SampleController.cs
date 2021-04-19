using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.Services;

namespace Viv2.API.AppInterface.Controllers
{
    /// <summary>
    /// Limited controller for adding and reviewing sample data.
    /// </summary>
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    public class SampleController : ControllerBase
    {
        private readonly IGetSamplesUseCase _sampleRetrieval;
        private readonly IClaimIdentityCompat _claimsCompat;
        
        public SampleController(IGetSamplesUseCase getSamplesUseCase, IClaimIdentityCompat claimsCompat)
        {
            _sampleRetrieval = getSamplesUseCase;
            _claimsCompat = claimsCompat;
        }

        [Authorize(Policy = PolicyNames.DaemonAccess)]
        [HttpPost("create")]
        public async Task<IActionResult> PostNewSample([FromBody] EnvSampleSubmission sample)
        {
            // Ensure model is correctly populated.
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            // Submit to core:
            var userId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User);
            if (userId == Guid.Empty) return BadRequest("Who are you?");

            return await Task.FromResult<IActionResult>(BadRequest("Not yet implemented"));
        }

        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet]
        public async Task<IActionResult> GetSamples()
        {
            var userId = Guid.Parse("55e3cd09-82b1-4383-aabf-766568b8f5b1"); 
            //Helpers.UserCompatHelper.UserGuidFromAuthenticatedContext(HttpContext);

            BasicPresenter<GenericDataResponse<ICollection<EnvDataSample>>> port = 
                new BasicPresenter<GenericDataResponse<ICollection<EnvDataSample>>>();

            DataAccessRequest<ICollection<EnvDataSample>> request = new DataAccessRequest<ICollection<EnvDataSample>>();
            request.UserId = userId;

            var success = await _sampleRetrieval.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response) : BadRequest();
        }
    }
}