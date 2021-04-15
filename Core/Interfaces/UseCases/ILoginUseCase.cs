using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;

namespace Viv2.API.Core.Interfaces.UseCases
{
    /// <summary>
    /// Usecase for logging a user in given credentials in the form of LoginReqeust
    /// </summary>
    public interface ILoginUseCase : IUseCaseRequestHandler<LoginRequest, LoginResponse>
    {
    }
}
