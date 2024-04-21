using Blogger.Contracts.Requests;
using Blogger.Contracts.Responses;
using Refit;

namespace Blogger.Sdk;

public interface IIdentityApi
{
    [Post("/api/identity/register")]
    Task<ApiResponse<Response>> RegisterAsync([Body] RegisterModel registerModel);

    [Post("/api/identity/registerAdmin")]
    Task<ApiResponse<Response>> RegisterAdminAsync([Body] RegisterModel registerModel);

    [Post("/api/identity/login")]
    Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] LoginModel loginModel);
}
