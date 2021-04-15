using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class TokenExchangeRequest : IUseCaseRequest<LoginResponse>
    {
        /// <summary>
        /// The base-64 encoded refresh token to be used during this exchange.
        /// </summary>
        public string EncodedRefreshToken { get; set; }
        
        /// <summary>
        /// The ID of the user ot which the refresh token was assigned to.
        /// </summary>
        public string UserId { get; set; }
    }
}
