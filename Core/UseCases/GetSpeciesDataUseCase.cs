using System;
using System.Linq;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.UseCases
{
    public class GetSpeciesDataUseCase : IGetSpeciesDataUseCase
    {
        private readonly IPetStore _petStore;

        public GetSpeciesDataUseCase(IPetStore petStore)
        {
            _petStore = petStore;
        }
        
        public async Task<bool> Handle(DataAccessRequest<ISpecies> message, IOutboundPort<GenericDataResponse<ISpecies>> outputPort)
        {
            // no need to validate user - species data not user specific.
            var response = new GenericDataResponse<ISpecies>();
            
            switch (message.Strategy)
            {
                case DataAccessRequest<ISpecies>.AcquisitionStrategy.All:
                    response.Result = await _petStore.GetSpeciesInfo();
                    break;
                case DataAccessRequest<ISpecies>.AcquisitionStrategy.Range:
                    response.Result = (await _petStore.GetSpeciesInfo())
                        .Where(message.SelectionPredicate)
                        .ToArray();
                    break;
                case DataAccessRequest<ISpecies>.AcquisitionStrategy.Single:
                    response.Result = new[]
                        {(await _petStore.GetSpeciesInfo()).FirstOrDefault(message.SelectionPredicate)};
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(message));
            }
            
            outputPort.Handle(response);
            return true;
        }
    }
}