using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Application.Auth.Requests;
using GoodHamburger.Application.Auth.Responses;
using GoodHamburger.Application.Users.Interfaces;

namespace GoodHamburger.Application.Auth.Services
{
    public sealed class AuthService(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        IAccessTokenService accessTokenService) : IAuthService
    {
        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            var user = await users.GetByEmailAsync(request.Email.Trim(), ct);
            if (user is null || !user.IsActive || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            return new AuthResponse
            {
                AccessToken = accessTokenService.GenerateToken(user),
                ExpiresIn = accessTokenService.GetExpiresInSeconds(),
                User = new AuthenticatedUserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }
            };
        }
    }
}
