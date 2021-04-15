namespace Viv2.API.Core.Interfaces
{
    /// <summary>
    /// Represents an interaction going out of the core into either AppInterface or
    /// Infrastructure.
    /// E.g., a login process may make use of the LoginUseCase, which relates an attempt to login with
    /// login pass/fail out. In this example the construct that represents the login result is TResponse,
    /// and an IOutgoingPort is used to present the result.
    /// </summary>
    public interface IOutboundPort <in TUseCaseResponse>
    {
        void Handle(TUseCaseResponse response);
    }
}
