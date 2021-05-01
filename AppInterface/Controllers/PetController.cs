using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    [Authorize(Policy = PolicyNames.UserAccess)]
    public class PetController : ControllerBase
    {
        private readonly IClaimIdentityCompat _claimsCompat;
        private readonly IAddPetUseCase _addPetUseCase;
        private readonly IGetPetDataUseCase _getPetDataUseCase;
        private readonly IGetSamplesUseCase _sampleDataUseCase;
        
        public PetController(IClaimIdentityCompat claimIdentityCompat,
            IAddPetUseCase addPetUseCase,
            IGetPetDataUseCase getPetDataUseCase,
            IGetSamplesUseCase getSamplesUseCase)
        {
            _claimsCompat = claimIdentityCompat;
            _addPetUseCase = addPetUseCase;
            _getPetDataUseCase = getPetDataUseCase;
            _sampleDataUseCase = getSamplesUseCase;
        }

        [HttpGet("ids")]
        public async Task<IActionResult> GetIdsForUsersPets()
        {
            var userId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User);

            // fetch all pets for the given user.
            var request = new DataAccessRequest<IPet>
            {
                UserId = userId,
                Strategy = DataAccessRequest<IPet>.AcquisitionStrategy.All
            };
            var port = new BasicPresenter<GenericDataResponse<IPet>>();
            var success = await _getPetDataUseCase.Handle(request, port);
            
            if (!success) return BadRequest();
            
            // Transform into just IDs
            var idList = port.Response.Result.Select(pet => pet.Id).ToImmutableList();
            return new OkObjectResult(idList);
        }
        
        /// <summary>
        /// Fetches and retrieves full info on the pet, granted that the user
        /// has access to the specified pet.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetDetails(int id)
        {
            var userId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User);
            
            // Just request all for the user while waiting for improvements on Core data access
            // logic.
            var request = new DataAccessRequest<IPet>
            {
                UserId = userId,
                // TODO: when capable swap to something that'd enable specifying the exact
                //       entity to access.
                Strategy = DataAccessRequest<IPet>.AcquisitionStrategy.All
            };
            var port = new BasicPresenter<GenericDataResponse<IPet>>();

            var success = await _getPetDataUseCase.Handle(request, port);
            if (!success) return BadRequest();
            
            // search for the requested Id:
            var pet = port.Response.Result.FirstOrDefault(p => p.Id == id);
            
            return (pet != null) ? new OkObjectResult(PetDto.From(pet)) : BadRequest();
        }

        /// <summary>
        /// Loads preliminary data to display for the specified pet.
        /// The content of the data set is specified by <see cref="PreliminaryPetData"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/prelim")]
        public async Task<IActionResult> GetPreliminaryDataFor(int id)
        {
            var userId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User);
            
            // Fetch the pet specified by ID
            var petRequest = new DataAccessRequest<IPet>
            {
                UserId = userId,
                Strategy = DataAccessRequest<IPet>.AcquisitionStrategy.Single,
                SelectionPredicate = (pet => pet.Id == id)
            };
            var petPort = new BasicPresenter<GenericDataResponse<IPet>>();
            var success = await _getPetDataUseCase.Handle(petRequest, petPort);

            if (!success) return BadRequest();

            // Fetch samples for the pet
            var sampleRequest = new DataAccessRequest<IEnvDataSample>
            {
                UserId = userId,
                Strategy = DataAccessRequest<IEnvDataSample>.AcquisitionStrategy.Range,
                SelectionPredicate = (sample => sample.Occupant?.Id == id)
            };
            var samplePort = new BasicPresenter<GenericDataResponse<IEnvDataSample>>();
            success = await _sampleDataUseCase.Handle(sampleRequest, samplePort);

            if (!success) return BadRequest();

            var latestSample = (samplePort.Response.Result.Count > 0)
                ? SampleDto.From(
                    samplePort.Response.Result
                        .Aggregate((agg, cur) =>
                            (agg.Captured ?? DateTime.MinValue) > (cur.Captured ?? DateTime.MinValue) ? agg : cur
                        ))
                : null;
            
            // compose final DTO
            var dto = new PreliminaryPetData
            {
                Pet = PetDto.From(petPort.Response.Result.First()),
                
                // NOTE: while this is (nearly) a one-liner, it causes frequent re-evaluation of .Captured
                // properties, which is unideal.
                LatestSample = latestSample
            };

            return new OkObjectResult(dto);
        }
        
        /// <summary>
        /// Add a new pet, associated to the current user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> CreateNewPet([FromBody] NewPetForm petInfo)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(petInfo.Name)) 
                return BadRequest(ModelState);
            
            // Convert into core Dto.NewPetRequest.
            var request = new CreatePetRequest
            {
                Name = petInfo.Name,
                SpeciesType = petInfo.SpeciesId,
                Morph = petInfo.Morph,
                User = _claimsCompat.ExtractFirstIdClaim(HttpContext.User)
            };

            var port = new BasicPresenter<NewEntityResponse<int>>();
            var success = await _addPetUseCase.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response.Id) : BadRequest();
        }
    }
}