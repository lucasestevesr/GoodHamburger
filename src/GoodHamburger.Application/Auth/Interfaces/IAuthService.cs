using GoodHamburger.Application.Auth.Requests;
using GoodHamburger.Application.Auth.Responses;

namespace GoodHamburger.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct);
    }
}
