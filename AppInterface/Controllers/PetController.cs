using System;
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
        private readonly IMigratePetUseCase _migratePetUseCase;
        private readonly IPetImageUseCase _imageUseCase;
        
        public PetController(IClaimIdentityCompat claimIdentityCompat,
            IAddPetUseCase addPetUseCase,
            IGetPetDataUseCase getPetDataUseCase,
            IGetSamplesUseCase getSamplesUseCase,
            IMigratePetUseCase migratePetUseCase,
            IPetImageUseCase imageUseCase)
        {
            _claimsCompat = claimIdentityCompat;
            _addPetUseCase = addPetUseCase;
            _getPetDataUseCase = getPetDataUseCase;
            _sampleDataUseCase = getSamplesUseCase;
            _migratePetUseCase = migratePetUseCase;
            _imageUseCase = imageUseCase;
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
        /// Fetches the env samples that are associated to the specified pet by ID
        /// within the range [start, end)
        /// </summary>
        /// <param name="id">The Pet ID whose samples are being fetched</param>
        /// <param name="start">start of the capture time range, inclusive</param>
        /// <param name="end">end of the capture time range, exclusive</param>
        /// <returns></returns>
        [HttpGet("{id}/samples")]
        public async Task<IActionResult> GetSamplesPage(int id, DateTime start, DateTime end)
        {
            if (start.Equals(DateTime.MinValue) || end.Equals(DateTime.MinValue)) return BadRequest(ModelState);

            if (end < start)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }

            var request = new DataAccessRequest<IEnvDataSample>()
            {
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                Strategy = DataAccessRequest<IEnvDataSample>.AcquisitionStrategy.Range,
                SelectionPredicate = sample =>
                {
                    var captureTime = sample.Captured;
                    var gteStart = start <= captureTime;
                    var ltEnd = captureTime < end;
                    var matchingPet = sample.Occupant?.Id == id;
                    return matchingPet && gteStart && ltEnd;
                }
            };

            var port = new BasicPresenter<GenericDataResponse<IEnvDataSample>>();
            var success = await _sampleDataUseCase.Handle(request, port);

            if (!success) return BadRequest();
            return new OkObjectResult(port.Response.Result.Select(SampleDto.From));
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

        /// <summary>
        /// Migrate the pet specified by `id
        ///
        /// Note: the destination env must be empty or occupied by a
        ///         pet that is directly owned by the active user.
        /// </summary>
        /// <param name="id">Id specifying the pet to migrate</param>
        /// <param name="toEnv">Env Id specifying where to migrate the pet</param>
        /// <returns></returns>
        [HttpPost("{id}/migrate")]
        public async Task<IActionResult> MigratePet(int id, Guid toEnv)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var request = new MigratePetRequest
            {
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                PetId = id,
                EnvId = toEnv
            };

            var port = new BasicPresenter<BlankResponse>();
            var result = await _migratePetUseCase.Handle(request, port);

            return (result) ? Ok() : BadRequest();
        }

        [HttpPut("{id}/image")]
        public async Task<IActionResult> UpdateProfileImage(int id)
        {
            // Ensure content type is supported 
            // TODO: look at applying some sort of 'upstream' filter on this instead of checking here.
            //       (it'll probably help with preflight requests and the like)
            var supportedMimeTypes = new [] {"image/png", "image/jpeg"};
            if (!supportedMimeTypes.Any(x => x.Equals(Request.ContentType))) 
                return new UnsupportedMediaTypeResult();
            
            var request = new PetImageRequest
            {
                Update = true,
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                PetId = id,
                Content = Request.Body,
                MimeType = Request.ContentType
            };

            var port = new BasicPresenter<BlobUriResponse>();

            var success = await _imageUseCase.Handle(request, port);
            if (success)
                return new OkObjectResult(port.Response)
                {
                    StatusCode = 201
                };

            return BadRequest();
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetPetImage(int id)
        {
            var request = new PetImageRequest
            {
                Update = false,
                UserId = _claimsCompat.ExtractFirstIdClaim(HttpContext.User),
                PetId = id,
                Content = null
            };

            var port = new BasicPresenter<BlobUriResponse>();

            var success = await _imageUseCase.Handle(request, port);
            return (success) ? new RedirectResult(port.Response.Uri.ToString()) : NotFound();
        }
    }
}