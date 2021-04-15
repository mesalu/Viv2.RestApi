using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    /// <summary>
    /// Contains necessary data to invoke the Core's login use case.
    /// </summary>
    public class LoginRequest : IUseCaseRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}