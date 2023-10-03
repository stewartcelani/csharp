using Refit;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;

namespace Tweetbook.SDK;

public interface IIdentityApi
{
    [Post("/identity/register")]
    Task<ApiResponse<AuthSuccessResponse>> RegisterAsync([Body] UserRegistrationRequest registrationRequest);

    [Post("/identity/login")]
    Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] UserLoginRequest loginRequest);

    [Post("/identity/refresh")]
    Task<ApiResponse<AuthSuccessResponse>> RefreshAsync([Body] RefreshTokenRequest refreshTokenRequest);
}