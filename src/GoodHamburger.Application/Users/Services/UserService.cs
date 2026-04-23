using GoodHamburger.Application.Common.Exceptions;
using GoodHamburger.Application.Common.Interfaces;
using GoodHamburger.Application.Identity;
using GoodHamburger.Application.Identity.Interfaces;
using GoodHamburger.Application.Identity.Requests;
using GoodHamburger.Application.Identity.Responses;
using GoodHamburger.Application.Users.Interfaces;
using GoodHamburger.Application.Users.Mappings;
using GoodHamburger.Application.Users.Requests;
using GoodHamburger.Application.Users.Responses;

namespace GoodHamburger.Application.Users.Services
{
    public sealed class UserService(
        IIdentityService identityService,
        ICurrentUser currentUser) : IUserService
    {
        public async Task<IReadOnlyList<UserResponse>> ListAsync(CancellationToken ct)
        {
            var userList = await identityService.ListUsersAsync(ct);
            var responses = new List<UserResponse>(userList.Count);

            foreach (var user in userList)
            {
                var roles = await identityService.GetRolesAsync(user.Id, ct);
                responses.Add(user.ToResponse(roles.FirstOrDefault() ?? string.Empty));
            }

            return responses;
        }

        public async Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            var user = await GetUserAsync(userId, ct);
            var roles = await identityService.GetRolesAsync(user.Id, ct);

            return user.ToResponse(roles.FirstOrDefault() ?? string.Empty);
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            await EnsureEmailAvailableAsync(request.Email, null, ct);
            await EnsureRoleExistsAsync(request.Role, ct);

            var identityRequest = new IdentityCreateUserRequest
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Password = request.Password,
                Role = request.Role,
                IsActive = request.IsActive
            };

            var user = await identityService.CreateUserAsync(identityRequest, ct);

            return user.ToResponse(request.Role);
        }

        public async Task<UserResponse> CreateAttendantAsync(CreateAttendantRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);

            EnsureManagerOrAdmin();
            await EnsureEmailAvailableAsync(request.Email, null, ct);
            await EnsureRoleExistsAsync(IdentityRoles.Attendant, ct);

            var identityRequest = new IdentityCreateUserRequest
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Password = request.Password,
                Role = IdentityRoles.Attendant,
                IsActive = request.IsActive
            };

            var user = await identityService.CreateUserAsync(identityRequest, ct);

            return user.ToResponse(IdentityRoles.Attendant);
        }

        public async Task<UserResponse> UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            var user = await GetUserAsync(userId, ct);

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            if (!string.Equals(user.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase))
            {
                await EnsureEmailAvailableAsync(normalizedEmail, userId, ct);
            }

            await EnsureRoleExistsAsync(request.Role, ct);

            var identityRequest = new IdentityUpdateUserRequest
            {
                Id = userId,
                Name = request.Name.Trim(),
                Email = normalizedEmail,
                Role = request.Role,
                IsActive = request.IsActive,
                Password = request.Password
            };

            var updatedUser = await identityService.UpdateUserAsync(identityRequest, ct);

            return updatedUser.ToResponse(request.Role);
        }

        public async Task DeleteAsync(Guid userId, CancellationToken ct)
        {
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            await identityService.DeleteUserAsync(userId, ct);
        }

        public async Task DeleteAttendantAsync(Guid userId, CancellationToken ct)
        {
            EnsureManagerOrAdmin();
            EnsureNotEmpty(userId, nameof(userId), "UserId inválido.");

            var user = await GetUserAsync(userId, ct);
            var roles = await identityService.GetRolesAsync(user.Id, ct);
            var role = roles.FirstOrDefault();

            if (!string.Equals(role, IdentityRoles.Attendant, StringComparison.OrdinalIgnoreCase))
                throw new ForbiddenAccessException("Apenas usuários do tipo Attendant podem ser removidos por esta rota.");

            await identityService.DeleteUserAsync(userId, ct);
        }

        private async Task<IdentityUserResponse> GetUserAsync(Guid userId, CancellationToken ct)
        {
            var user = await identityService.GetUserByIdAsync(userId, ct);
            if (user is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return user;
        }

        private async Task EnsureEmailAvailableAsync(string email, Guid? currentUserId, CancellationToken ct)
        {
            var existing = await identityService.GetUserByEmailAsync(email.Trim().ToLowerInvariant(), ct);
            if (existing is null)
                return;

            if (currentUserId.HasValue && existing.Id == currentUserId.Value)
                return;

            throw new ArgumentException("Já existe um usuário cadastrado com este e-mail.", nameof(email));
        }

        private async Task EnsureRoleExistsAsync(string role, CancellationToken ct)
        {
            if (!await identityService.RoleExistsAsync(role, ct))
                throw new ArgumentException($"Perfil inválido: '{role}'.", nameof(role));
        }

        private void EnsureManagerOrAdmin()
        {
            if (!currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(currentUser.Role))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            if (!IsManagerOrAdmin(currentUser.Role))
                throw new ForbiddenAccessException("Usuário não possui permissão para esta operação.");
        }

        private static bool IsManagerOrAdmin(string role)
        {
            return string.Equals(role, IdentityRoles.Manager, StringComparison.OrdinalIgnoreCase)
                || string.Equals(role, IdentityRoles.Admin, StringComparison.OrdinalIgnoreCase);
        }

        private static void EnsureNotEmpty(Guid value, string parameterName, string message)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(message, parameterName);
        }
    }
}
