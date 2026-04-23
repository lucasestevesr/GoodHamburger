using GoodHamburger.Web.Auth.Responses;
using GoodHamburger.Web.Identity;

namespace GoodHamburger.Web.Security;

public sealed class AuthSession
{
    public event Action? Changed;

    public string? AccessToken { get; private set; }

    public AuthenticatedUserResponse? User { get; private set; }

    public bool IsInitialized { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken) && User is not null;

    public bool IsAdmin => IsInRole(IdentityRoles.Admin);

    public bool IsManager => IsInRole(IdentityRoles.Manager);

    public bool IsAttendant => IsInRole(IdentityRoles.Attendant);

    public bool CanManageOrders => IsInRole(IdentityRoles.Admin, IdentityRoles.Manager, IdentityRoles.Attendant);

    public bool CanViewProducts => CanManageOrders;

    public bool CanManageProducts => IsInRole(IdentityRoles.Admin, IdentityRoles.Manager);

    public bool CanManageUsers => IsInRole(IdentityRoles.Admin);

    public bool CanManageAttendants => IsInRole(IdentityRoles.Admin, IdentityRoles.Manager);

    public async Task InitializeAsync(AuthSessionStorage storage)
    {
        if (IsInitialized)
            return;

        var response = await storage.GetAsync();
        if (response is not null)
            ApplySession(response);

        IsInitialized = true;
        Changed?.Invoke();
    }

    public async Task SignInAsync(AuthResponse response, AuthSessionStorage storage)
    {
        ApplySession(response);
        IsInitialized = true;
        await storage.SetAsync(response);
        Changed?.Invoke();
    }

    public async Task SignOutAsync(AuthSessionStorage storage)
    {
        ClearSession();
        IsInitialized = true;
        await storage.ClearAsync();
        Changed?.Invoke();
    }

    public void MarkInitialized()
    {
        IsInitialized = true;
        Changed?.Invoke();
    }

    public bool IsInRole(params string[] roles)
    {
        if (User is null || string.IsNullOrWhiteSpace(User.Role))
            return false;

        return roles.Any(role => string.Equals(role, User.Role, StringComparison.OrdinalIgnoreCase));
    }

    private void ApplySession(AuthResponse response)
    {
        AccessToken = response.AccessToken;
        User = response.User;
    }

    private void ClearSession()
    {
        AccessToken = null;
        User = null;
    }
}
