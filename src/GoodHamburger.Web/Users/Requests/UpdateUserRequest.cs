using GoodHamburger.Web.Identity;

namespace GoodHamburger.Web.Users.Requests;

public sealed class UpdateUserRequest
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = IdentityRoles.Attendant;

    public bool IsActive { get; set; }

    public string? Password { get; set; }
}
