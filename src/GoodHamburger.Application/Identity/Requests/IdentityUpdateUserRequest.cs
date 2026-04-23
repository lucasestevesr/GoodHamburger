namespace GoodHamburger.Application.Identity.Requests
{
    public sealed class IdentityUpdateUserRequest
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string Role { get; init; } = string.Empty;

        public bool IsActive { get; init; }

        public string? Password { get; init; }
    }
}
