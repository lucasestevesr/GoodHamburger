using GoodHamburger.Application.Identity.Interfaces;
using GoodHamburger.Application.Identity.Requests;
using GoodHamburger.Application.Identity.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infra.Identity.Services
{
    public sealed class IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager) : IIdentityService
    {
        public async Task<IReadOnlyList<IdentityUserResponse>> ListUsersAsync(CancellationToken ct)
        {
            var users = await userManager.Users
                .AsNoTracking()
                .OrderBy(user => user.Name)
                .ToListAsync(ct);

            return users.Select(MapUser).ToList();
        }

        public async Task<IdentityUserResponse?> GetUserByIdAsync(Guid userId, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return user is null ? null : MapUser(user);
        }

        public async Task<IdentityUserResponse?> GetUserByEmailAsync(string email, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(email.Trim());
            return user is null ? null : MapUser(user);
        }

        public async Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return (await userManager.GetRolesAsync(user)).ToList();
        }

        public async Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return false;

            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> RoleExistsAsync(string role, CancellationToken ct)
        {
            return await roleManager.RoleExistsAsync(role);
        }

        public async Task<IdentityUserResponse> CreateUserAsync(IdentityCreateUserRequest request, CancellationToken ct)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var user = new ApplicationUser
            {
                Name = request.Name.Trim(),
                UserName = normalizedEmail,
                Email = normalizedEmail,
                IsActive = request.IsActive,
                CreationDate = DateTimeOffset.UtcNow
            };

            EnsureIdentityResult(
                await userManager.CreateAsync(user, request.Password),
                "Erro ao criar usuário.");

            EnsureIdentityResult(
                await userManager.AddToRoleAsync(user, request.Role),
                "Erro ao definir perfil.");

            return MapUser(user);
        }

        public async Task<IdentityUserResponse> UpdateUserAsync(IdentityUpdateUserRequest request, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            user.Name = request.Name.Trim();
            user.UserName = normalizedEmail;
            user.Email = normalizedEmail;
            user.IsActive = request.IsActive;

            EnsureIdentityResult(
                await userManager.UpdateAsync(user),
                "Erro ao atualizar usuário.");

            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Count != 1 || !string.Equals(currentRoles[0], request.Role, StringComparison.OrdinalIgnoreCase))
            {
                EnsureIdentityResult(
                    await userManager.RemoveFromRolesAsync(user, currentRoles),
                    "Erro ao atualizar perfil.");

                EnsureIdentityResult(
                    await userManager.AddToRoleAsync(user, request.Role),
                    "Erro ao atualizar perfil.");
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                EnsureIdentityResult(
                    await userManager.ResetPasswordAsync(user, token, request.Password),
                    "Erro ao atualizar senha.");
            }

            return MapUser(user);
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken ct)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            EnsureIdentityResult(
                await userManager.DeleteAsync(user),
                "Erro ao remover usuário.");
        }

        private static IdentityUserResponse MapUser(ApplicationUser user)
        {
            return new IdentityUserResponse
            {
                Id = user.Id,
                Name = user.Name ?? user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                IsActive = user.IsActive,
                CreationDate = user.CreationDate
            };
        }

        private static void EnsureIdentityResult(IdentityResult result, string message)
        {
            if (result.Succeeded)
                return;

            var details = string.Join(", ", result.Errors.Select(error => error.Description));
            throw new ArgumentException($"{message} {details}");
        }
    }
}
