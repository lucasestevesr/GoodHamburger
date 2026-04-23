using GoodHamburger.Web.Infrastructure.Http;
using GoodHamburger.Web.Users.Requests;
using GoodHamburger.Web.Users.Responses;

namespace GoodHamburger.Web.Users;

public sealed class UsersApiClient(ApiHttpClient api)
{
    public Task<IReadOnlyList<UserResponse>> ListAsync(CancellationToken ct = default)
    {
        return api.GetAsync<IReadOnlyList<UserResponse>>("api/v1/users", ct);
    }

    public Task<UserResponse> GetByIdAsync(Guid userId, CancellationToken ct = default)
    {
        return api.GetAsync<UserResponse>($"api/v1/users/{userId}", ct);
    }

    public Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        return api.PostAsync<UserResponse>("api/v1/users", request, ct);
    }

    public Task<UserResponse> CreateAttendantAsync(CreateAttendantRequest request, CancellationToken ct = default)
    {
        return api.PostAsync<UserResponse>("api/v1/users/attendants", request, ct);
    }

    public Task<UserResponse> UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken ct = default)
    {
        return api.PutAsync<UserResponse>($"api/v1/users/{userId}", request, ct);
    }

    public Task DeleteAsync(Guid userId, CancellationToken ct = default)
    {
        return api.DeleteAsync($"api/v1/users/{userId}", ct);
    }

    public Task DeleteAttendantAsync(Guid userId, CancellationToken ct = default)
    {
        return api.DeleteAsync($"api/v1/users/attendants/{userId}", ct);
    }
}
