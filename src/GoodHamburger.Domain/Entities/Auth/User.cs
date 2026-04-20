using GoodHamburger.Domain.Entities.Base;

namespace GoodHamburger.Domain.Entities.Auth
{
    public sealed class User : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; }
    }
}
