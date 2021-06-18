using System;
using System.Collections.Generic;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Interfaces.UseCases
{
    [Obsolete("Use ISampleAcquisitionUseCase instead.")]
    public interface IGetSamplesUseCase : IUseCaseRequestHandler<DataAccessRequest<IEnvDataSample>, GenericDataResponse<IEnvDataSample>>
    {
        
    }
}