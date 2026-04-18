using GoodHamburger.Domain.Entities.Base;

namespace GoodHamburger.Domain.Entities.Auth
{
    public sealed class User : BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }

        public bool IsActive { get; set; }
    }
}
