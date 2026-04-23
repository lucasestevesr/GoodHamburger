namespace GoodHamburger.Application.Identity.Requests
{
    public sealed class IdentityCreateUserRequest
    {
        public string Name { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;

        public string Role { get; init; } = string.Empty;

        public bool IsActive { get; init; }
    }
}
