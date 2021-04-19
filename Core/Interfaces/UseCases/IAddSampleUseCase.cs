using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;

namespace Viv2.API.Core.Interfaces.UseCases
{
    public interface IAddSampleUseCase : IUseCaseRequestHandler<NewSampleRequest, BlankResponse>
    {
        
    }
}