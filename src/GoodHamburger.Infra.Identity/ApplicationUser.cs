using Microsoft.AspNetCore.Identity;

namespace GoodHamburger.Infra.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
