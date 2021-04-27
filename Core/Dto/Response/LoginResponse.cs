using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Dto.Response
{
    public class LoginResponse
    {
        public AccessToken AccessToken { get; init; }
        public RefreshToken RefreshToken { get; init; }
    }
}
