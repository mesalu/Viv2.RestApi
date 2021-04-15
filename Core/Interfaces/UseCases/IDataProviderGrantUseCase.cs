using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;

namespace Viv2.API.Core.Interfaces.UseCases
{
    /// <summary>
    /// Use case for creating a token set suitable for a long-running data provider.
    /// Data providers have different access levels than a typical user, and are intended to
    /// be long running daemon applications.
    /// </summary>
    public interface IDataProviderGrantUseCase : IUseCaseRequestHandler<ProviderGrantRequest, LoginResponse>
    {
        
    }
}
