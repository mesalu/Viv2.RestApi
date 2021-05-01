using System.Collections.Generic;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;

using TContent = Viv2.API.Core.ProtoEntities.IPet;

namespace Viv2.API.Core.Interfaces.UseCases
{
    public interface IGetPetDataUseCase : IUseCaseRequestHandler<DataAccessRequest<TContent>, GenericDataResponse<TContent>>
    {
        
    }
}