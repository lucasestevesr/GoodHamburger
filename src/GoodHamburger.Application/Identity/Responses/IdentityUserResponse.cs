namespace GoodHamburger.Application.Identity.Responses
{
    public sealed class IdentityUserResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public bool IsActive { get; init; }

        public DateTimeOffset CreationDate { get; init; }
    }
}
