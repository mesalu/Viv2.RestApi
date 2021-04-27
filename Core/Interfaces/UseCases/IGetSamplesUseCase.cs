using System.Collections.Generic;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Interfaces.UseCases
{
    public interface IGetSamplesUseCase : IUseCaseRequestHandler<DataAccessRequest<ICollection<IEnvDataSample>>, GenericDataResponse<ICollection<IEnvDataSample>>>
    {
        
    }
}