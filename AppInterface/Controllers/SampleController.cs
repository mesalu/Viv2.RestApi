using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;
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
        private readonly IAddSampleUseCase _addSample;
        private readonly IClaimIdentityCompat _claimsCompat;
        
        public SampleController(IGetSamplesUseCase getSamplesUseCase, IClaimIdentityCompat claimsCompat,
            IAddSampleUseCase addSample)
        {
            _sampleRetrieval = getSamplesUseCase;
            _claimsCompat = claimsCompat;
            _addSample = addSample;
        }

        [Authorize(Policy = PolicyNames.DaemonAccess)]
        [HttpPost]
        public async Task<IActionResult> PostNewSample([FromBody] EnvSampleSubmission sample)
        {
            // Ensure model is correctly populated.
            if (!ModelState.IsValid
                || sample.Environment == Guid.Empty) return BadRequest(ModelState);
            
            // Submit to core:
            var userId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User);
            if (userId == Guid.Empty) return BadRequest("Who are you?");

            var request = new NewSampleRequest
            {
                Sample = sample,
                UserId = userId
            };
            var port = new BasicPresenter<BlankResponse>();
            
            var success = await _addSample.Handle(request, port);
            return (success) ? Ok() : BadRequest("Failed to post sample");
        }

        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet]
        public async Task<IActionResult> GetSamples()
        {
            var userId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User);

            BasicPresenter<GenericDataResponse<ICollection<IEnvDataSample>>> port = 
                new BasicPresenter<GenericDataResponse<ICollection<IEnvDataSample>>>();

            DataAccessRequest<ICollection<IEnvDataSample>> request = new DataAccessRequest<ICollection<IEnvDataSample>>();
            request.UserId = userId;

            var success = await _sampleRetrieval.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response) : BadRequest();
        }
    }
}