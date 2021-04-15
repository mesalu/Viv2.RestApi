using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.AppInterface.Ports
{
    public class LoginAttemptPort : IOutboundPort<LoginResponse>
    {
        public LoginResponse Response { get; private set; }
        public void Handle(LoginResponse response)
        {
            Response = response;
        }
    }
}
