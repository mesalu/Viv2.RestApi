using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Dto;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Constants;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.Services;

namespace Viv2.API.AppInterface.Controllers
{
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    [Authorize(Policy = PolicyNames.UserAccess)]
    public class PetController : ControllerBase
    {
        private readonly IClaimIdentityCompat _claimsCompat;
        private readonly IAddPetUseCase _addPetUseCase;
        
        public PetController(IClaimIdentityCompat claimIdentityCompat,
            IAddPetUseCase addPetUseCase)
        {
            _claimsCompat = claimIdentityCompat;
            _addPetUseCase = addPetUseCase;
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
            var request = new NewPetRequest
            {
                Name = petInfo.Name,
                SpeciesType = petInfo.SpeciesId,
                Morph = petInfo.Morph,
                User = _claimsCompat.ExtractFirstIdClaim(HttpContext.User)
            };

            var port = new BasicPresenter<NewEntityResponse<string>>();
            var success = await _addPetUseCase.Handle(request, port);

            return (success) ? Ok() : BadRequest();
        }
    }
}