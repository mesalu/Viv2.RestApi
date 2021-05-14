using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Interfaces.UseCases
{
    public interface IUpdateNodeUseCase : IUseCaseRequestHandler<NodeUpdateRequest, GenericDataResponse<IController>>
    {
        
    }
}