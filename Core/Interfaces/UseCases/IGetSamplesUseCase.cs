using System.Collections.Generic;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Entities;

namespace Viv2.API.Core.Interfaces.UseCases
{
    public interface IGetSamplesUseCase : IUseCaseRequestHandler<DataAccessRequest<ICollection<EnvDataSample>>, GenericDataResponse<ICollection<EnvDataSample>>>
    {
        
    }
}