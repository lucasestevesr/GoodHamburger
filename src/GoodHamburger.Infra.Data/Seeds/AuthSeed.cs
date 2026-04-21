using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Infra.Data.Seeds
{
    internal static class AuthSeed
    {
        private static readonly DateTimeOffset SeedDate = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        internal static readonly Guid AdminUserId = new("5a8f84ba-46ed-4d8a-a556-7b9db2494c41");
        internal static readonly Guid ManagerUserId = new("b153c761-3122-4eb8-bb26-297315846b4c");
        internal static readonly Guid AttendantUserId = new("5e918c04-1634-41d8-8d7e-96f4059b8163");

        private const string AdminPasswordHash = "AQAAAAIAAYagAAAAEAECAwQFBgcICQoLDA0ODxCc1N01GG8CSt95mAzGrOnxFMB+OMut8DMObIEOtQ4qYg==";
        private const string ManagerPasswordHash = "AQAAAAIAAYagAAAAEBESExQVFhcYGRobHB0eHyD9Metf/8aK26Bucm/TMizq6kR+buFqIKvjtSdORrFiqw==";
        private const string AttendantPasswordHash = "AQAAAAIAAYagAAAAECEiIyQlJicoKSorLC0uLzCtdWvuomFlDAvcgACGSCjIhQZs2TMhgaf/oJ1Qm92jQQ==";

        internal static IEnumerable<User> Users => new[]
        {
            new User
            {
                Id = AdminUserId,
                CreationDate = SeedDate,
                Name = "Admin User",
                Email = "admin@goodhamburger.com",
                PasswordHash = AdminPasswordHash,
                Role = UserRole.Admin,
                IsActive = true
            },
            new User
            {
                Id = ManagerUserId,
                CreationDate = SeedDate,
                Name = "Manager User",
                Email = "manager@goodhamburger.com",
                PasswordHash = ManagerPasswordHash,
                Role = UserRole.Manager,
                IsActive = true
            },
            new User
            {
                Id = AttendantUserId,
                CreationDate = SeedDate,
                Name = "Attendant User",
                Email = "attendant@goodhamburger.com",
                PasswordHash = AttendantPasswordHash,
                Role = UserRole.Attendant,
                IsActive = true
            }
        };
    }
}
