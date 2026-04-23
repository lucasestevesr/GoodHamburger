using GoodHamburger.Application.Identity.Requests;
using GoodHamburger.Application.Identity.Responses;

namespace GoodHamburger.Application.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<IReadOnlyList<IdentityUserResponse>> ListUsersAsync(CancellationToken ct);
        Task<IdentityUserResponse?> GetUserByIdAsync(Guid userId, CancellationToken ct);
        Task<IdentityUserResponse?> GetUserByEmailAsync(string email, CancellationToken ct);
        Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken ct);
        Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken ct);
        Task<bool> RoleExistsAsync(string role, CancellationToken ct);
        Task<IdentityUserResponse> CreateUserAsync(IdentityCreateUserRequest request, CancellationToken ct);
        Task<IdentityUserResponse> UpdateUserAsync(IdentityUpdateUserRequest request, CancellationToken ct);
        Task DeleteUserAsync(Guid userId, CancellationToken ct);
    }
}
