using System.Threading.Tasks;

namespace Viv2.API.Core.Interfaces
{
    /// <summary>
    /// Represents the action taken when a use case is requested. In a way, extensions of this interface
    /// tie use case reqeusts to use case responses.
    /// E.g. an application may request the invocation of the 'login' use case, which consumes a LoginRequest
    /// and provides a LoginReponse once processing has been handled by the correct extension of this interface.
    /// </summary>
    public interface IUseCaseRequestHandler<in TUseCaseRequest, TUseCaseResponse> where TUseCaseRequest : IUseCaseRequest<TUseCaseResponse>
    {
        Task<bool> Handle(TUseCaseRequest message, IOutboundPort<TUseCaseResponse> outputPort);
    }
}