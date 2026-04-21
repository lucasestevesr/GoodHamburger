namespace GoodHamburger.Application.Users.Responses
{
    public sealed class UserResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTimeOffset CreationDate { get; set; }
    }
}
