using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Application.Auth.Requests;
using GoodHamburger.Application.Auth.Responses;
using GoodHamburger.Application.Identity.Interfaces;

namespace GoodHamburger.Application.Auth.Services
{
    public sealed class AuthService(
        IIdentityService identityService,
        IAccessTokenService accessTokenService) : IAuthService
    {
        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            var user = await identityService.GetUserByEmailAsync(request.Email.Trim(), ct);
            if (user is null || !user.IsActive)
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            var passwordValid = await identityService.CheckPasswordAsync(user.Id, request.Password, ct);
            if (!passwordValid)
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            var roles = await identityService.GetRolesAsync(user.Id, ct);

            return new AuthResponse
            {
                AccessToken = accessTokenService.GenerateAccessToken(
                    user.Id,
                    user.Email,
                    roles),
                ExpiresIn = accessTokenService.GetExpiresInSeconds(),
                User = new AuthenticatedUserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? string.Empty
                }
            };
        }
    }
}
