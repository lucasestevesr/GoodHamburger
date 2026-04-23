using GoodHamburger.Web.Auth.Requests;
using GoodHamburger.Web.Auth.Responses;
using GoodHamburger.Web.Infrastructure.Http;

namespace GoodHamburger.Web.Auth;

public sealed class AuthApiClient(ApiHttpClient api)
{
    public Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        return api.PostAnonymousAsync<AuthResponse>("api/v1/auth/login", request, ct);
    }
}
