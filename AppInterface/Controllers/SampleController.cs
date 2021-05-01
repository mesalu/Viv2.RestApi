using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Dto;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

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
            var port = new BasicPresenter<GenericDataResponse<IEnvDataSample>>();
            var request = new DataAccessRequest<IEnvDataSample>
            {
                UserId = userId,
                Strategy = DataAccessRequest<IEnvDataSample>.AcquisitionStrategy.All
            };
            var success = await _sampleRetrieval.Handle(request, port);
            return (success) ? new OkObjectResult(port.Response.Result.Select(SampleDto.From)) : BadRequest();
        }
        
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet("slice")]
        public async Task<IActionResult> GetSamples(string from, string to)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(from)) return BadRequest(ModelState);

            DateTime a, b;
            try
            {
                a = DateTime.Parse(from);
                b = (to != null) ? DateTime.Parse(to) : DateTime.MaxValue;
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }

            if (a > b)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }
            
            var userId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User);
            var port = new BasicPresenter<GenericDataResponse<IEnvDataSample>>();
            var request = new DataAccessRequest<IEnvDataSample>
            {
                UserId = userId,
                Strategy = DataAccessRequest<IEnvDataSample>.AcquisitionStrategy.Range,
                
                // NOTE: ISTR having issues with DateTime static values not having the unspecified
                // 'kind', as such we may have some issues with naive DateTimes here.
                SelectionPredicate = (sample => InDateTimeRange(sample.Captured, a, b))
            };
            var success = await _sampleRetrieval.Handle(request, port);
            return (success) ? new OkObjectResult(port.Response.Result.Select(SampleDto.From)) : BadRequest();
        }

        /// <summary>
        /// if x is valid, converts all inputs to UTC and ensures that x is
        /// somewhere in the inclusive range [a, b]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if x != null && a <= x <= b, false otherwise</returns>
        private static bool InDateTimeRange(DateTime? x, DateTime a, DateTime b)
        {
            if (x == null) return false;

            // Ensure all inputs are in UTC. (X should be, but doesn't hurt to scrub.)
            x = x.Value.ToUniversalTime();
            a = a.ToUniversalTime();
            b = b.ToUniversalTime();
            
            return a <= x && x <= b;
        }
    }
}