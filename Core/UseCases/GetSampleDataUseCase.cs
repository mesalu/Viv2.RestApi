using System.Collections.Generic;
using System.Threading.Tasks;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Entities;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;
using Viv2.API.Core.Services;

namespace Viv2.API.Core.UseCases
{
    public class GetSampleDataUseCase : IGetSamplesUseCase
    {
        private readonly IPetBackingStore _backingStore;

        public GetSampleDataUseCase(IPetBackingStore backingStore)
        {
            _backingStore = backingStore;
        }
        
        public Task<bool> Handle(DataAccessRequest<ICollection<EnvDataSample>> message, IOutboundPort<GenericDataResponse<ICollection<EnvDataSample>>> outputPort)
        {
            // TODO
            
            // placeholder:
            List<EnvDataSample> samples = new List<EnvDataSample>();
            GenericDataResponse<ICollection<EnvDataSample>> response =
                new GenericDataResponse<ICollection<EnvDataSample>>();
            response.Result = samples;
            
            outputPort.Handle(response);
            return Task.FromResult(true);
        }
    }
}