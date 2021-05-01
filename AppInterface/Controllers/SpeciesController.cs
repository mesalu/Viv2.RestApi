using System;
using System.Collections.Generic;
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

namespace Viv2.API.AppInterface.Controllers
{
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    public class SpeciesController : ControllerBase
    {
        private readonly IAddSpeciesUseCase _addSpecies;
        private readonly IGetSpeciesDataUseCase _getSpeciesData;

        public SpeciesController(IAddSpeciesUseCase addSpecies, IGetSpeciesDataUseCase getSpeciesData)
        {
            _getSpeciesData = getSpeciesData;
            _addSpecies = addSpecies;
        }

        [Authorize(Roles = RoleValues.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateNewSpecies([FromBody] CreateSpeciesRequest model)
        {
            if (!ModelState.IsValid
                || string.IsNullOrWhiteSpace(model.Name)
                || string.IsNullOrEmpty(model.ScientificName)
                || Math.Abs(model.Latitude) > 90.0
                || Math.Abs(model.Longitude) > 180.0) return BadRequest(ModelState);

            var port = new BasicPresenter<NewEntityResponse<int>>();
            var success = await _addSpecies.Handle(model, port);

            return (success) ? new OkObjectResult(port.Response.Id) : BadRequest();
        }
        
        [Authorize(Policy = PolicyNames.UserAccess)]
        [HttpGet]
        public async Task<IActionResult> GetAllSpecies()
        {
            // Species are not user-specific, so we won't need to specify that in the request.
            var request = new DataAccessRequest<ISpecies> { Strategy = DataAccessRequest<ISpecies>.AcquisitionStrategy.All };
            var port = new BasicPresenter<GenericDataResponse<ISpecies>>();
            var success = await _getSpeciesData.Handle(request, port);

            return (success) ? new OkObjectResult(port.Response.Result) : BadRequest();
        }
    }
}