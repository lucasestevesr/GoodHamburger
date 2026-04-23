using GoodHamburger.Web.Identity;

namespace GoodHamburger.Web.Users.Requests;

public sealed class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = IdentityRoles.Attendant;

    public bool IsActive { get; set; } = true;
}
