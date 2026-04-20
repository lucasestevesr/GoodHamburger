using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Infra.Data.Seeds
{
    internal static class AuthSeed
    {
        private static readonly DateTimeOffset SeedDate = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        internal static readonly Guid AdminUserId = new("5a8f84ba-46ed-4d8a-a556-7b9db2494c41");
        internal static readonly Guid ManagerUserId = new("b153c761-3122-4eb8-bb26-297315846b4c");
        internal static readonly Guid AttendantUserId = new("5e918c04-1634-41d8-8d7e-96f4059b8163");

        internal static IEnumerable<User> Users => new[]
        {
            new User
            {
                Id = AdminUserId,
                CreationDate = SeedDate,
                Name = "Admin User",
                Email = "admin@goodhamburger.com",
                PasswordHash = "S3cr3tP@ssw0rd",
                Role = UserRole.Admin,
                IsActive = true
            },
            new User
            {
                Id = ManagerUserId,
                CreationDate = SeedDate,
                Name = "Manager User",
                Email = "manager@goodhamburger.com",
                PasswordHash = "S3cr3tP@ssw0rd",
                Role = UserRole.Manager,
                IsActive = true
            },
            new User
            {
                Id = AttendantUserId,
                CreationDate = SeedDate,
                Name = "Attendant User",
                Email = "attendant@goodhamburger.com",
                PasswordHash = "S3cr3tP@ssw0rd",
                Role = UserRole.Attendant,
                IsActive = true
            }
        };
    }
}
