using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;

namespace Viv2.API.Core.Interfaces.UseCases
{
    /// <summary>
    /// Handles exchanging a refresh token for a new set of login information.
    /// If the given refresh token is valid, matches the specified user, is still valid, and will
    /// remain valid after its exchange, then the refresh token will appear - once again - as the RefreshToken
    /// property of LoginResponse. Otherwise (if only the last condition is false) a new refresh token will be
    /// minted and returned as part of the login response.
    /// </summary>
    public interface IRefreshTokenExchangeUseCase : IUseCaseRequestHandler<TokenExchangeRequest, LoginResponse>
    { }
}
