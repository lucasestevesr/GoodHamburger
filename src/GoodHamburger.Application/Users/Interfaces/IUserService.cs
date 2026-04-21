using GoodHamburger.Application.Users.Requests;
using GoodHamburger.Application.Users.Responses;

namespace GoodHamburger.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<UserResponse>> ListAsync(CancellationToken ct);
        Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken ct);
        Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct);
        Task<UserResponse> CreateAttendantAsync(CreateAttendantRequest request, CancellationToken ct);
        Task<UserResponse> UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken ct);
        Task DeleteAsync(Guid userId, CancellationToken ct);
        Task DeleteAttendantAsync(Guid userId, CancellationToken ct);
    }
}
