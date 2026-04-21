using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Application.Common.Exceptions;
using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Application.Users.Interfaces;
using GoodHamburger.Application.Users.Mappings;
using GoodHamburger.Application.Users.Requests;
using GoodHamburger.Application.Users.Responses;
using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Application.Users.Services
{
    public sealed class UserService(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        ICurrentUser currentUser) : IUserService
    {
        public async Task<IReadOnlyList<UserResponse>> ListAsync(CancellationToken ct)
        {
            var userList = await users.ListAsync(ct);
            return userList.Select(user => user.ToResponse()).ToList();
        }

        public async Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            var user = await GetUserAsync(userId, ct);
            return user.ToResponse();
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            await EnsureEmailAvailableAsync(request.Email, ct);

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                PasswordHash = passwordHasher.HashPassword(request.Password),
                Role = request.Role,
                IsActive = request.IsActive
            };

            await users.AddAsync(user, ct);
            await users.SaveChangesAsync(ct);

            return user.ToResponse();
        }

        public async Task<UserResponse> CreateAttendantAsync(CreateAttendantRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            EnsureManagerOrAdmin();
            await EnsureEmailAvailableAsync(request.Email, ct);

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                PasswordHash = passwordHasher.HashPassword(request.Password),
                Role = UserRole.Attendant,
                IsActive = request.IsActive
            };

            await users.AddAsync(user, ct);
            await users.SaveChangesAsync(ct);

            return user.ToResponse();
        }

        public async Task<UserResponse> UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            var user = await GetUserAsync(userId, ct);

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            if (!string.Equals(user.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase))
            {
                await EnsureEmailAvailableAsync(normalizedEmail, ct);
            }

            user.Name = request.Name.Trim();
            user.Email = normalizedEmail;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.PasswordHash = passwordHasher.HashPassword(request.Password);
            }

            await users.SaveChangesAsync(ct);

            return user.ToResponse();
        }

        public async Task DeleteAsync(Guid userId, CancellationToken ct)
        {
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            var user = await GetUserAsync(userId, ct);
            users.Remove(user);

            await users.SaveChangesAsync(ct);
        }

        public async Task DeleteAttendantAsync(Guid userId, CancellationToken ct)
        {
            EnsureManagerOrAdmin();
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            var user = await GetUserAsync(userId, ct);
            if (user.Role != UserRole.Attendant)
                throw new ForbiddenAccessException("Apenas usuários do tipo Attendant podem ser removidos por esta rota.");

            users.Remove(user);
            await users.SaveChangesAsync(ct);
        }

        private async Task<User> GetUserAsync(Guid userId, CancellationToken ct)
        {
            var user = await users.GetByIdAsync(userId, ct);
            if (user is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return user;
        }

        private async Task EnsureEmailAvailableAsync(string email, CancellationToken ct)
        {
            var emailInUse = await users.ExistsByEmailAsync(email.Trim().ToLowerInvariant(), ct);
            if (emailInUse)
                throw new ArgumentException("Já existe um usuário cadastrado com este e-mail.", nameof(email));
        }

        private void EnsureManagerOrAdmin()
        {
            if (!currentUser.IsAuthenticated || currentUser.Role is null)
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            if (currentUser.Role is not UserRole.Manager and not UserRole.Admin)
                throw new ForbiddenAccessException("Usuário não possui permissão para esta operação.");
        }

        private static void EnsureNotEmpty(Guid value, string parameterName, string message)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(message, parameterName);
        }
    }
}
