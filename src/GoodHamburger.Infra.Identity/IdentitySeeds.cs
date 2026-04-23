using GoodHamburger.Application.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GoodHamburger.Infra.Identity
{
    /// <summary>
    /// Seeds the database with initial roles and users for the application.
    /// </summary>
    public static class IdentitySeeds
    {
        private const string DefaultPassword = "S3cr3tP@ssw0rd";
        public static async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
        {
            foreach (var role in IdentityRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    if (!result.Succeeded)
                        throw new InvalidOperationException(
                            string.Join(", ", result.Errors.Select(error => error.Description)));
                }
            }
        }

        public static async Task SeedUsers(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            var defaultPassword = configuration["Seed:DefaultPassword"] ?? DefaultPassword;
            var users = new[]
            {
                new
                {
                    Email = configuration["Seed:AdminEmail"] ?? "admin@goodhamburger.com",
                    Name = "Admin User",
                    Role = IdentityRoles.Admin
                },
                new
                {
                    Email = configuration["Seed:ManagerEmail"] ?? "manager@goodhamburger.com",
                    Name = "Manager User",
                    Role = IdentityRoles.Manager
                },
                new
                {
                    Email = configuration["Seed:AttendantEmail"] ?? "attendant@goodhamburger.com",
                    Name = "Attendant User",
                    Role = IdentityRoles.Attendant
                }
            };

            foreach (var seedUser in users)
            {
                var user = await userManager.FindByEmailAsync(seedUser.Email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Name = seedUser.Name,
                        UserName = seedUser.Email,
                        Email = seedUser.Email,
                        EmailConfirmed = true,
                        IsActive = true,
                        CreationDate = DateTimeOffset.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, defaultPassword);

                    if (!result.Succeeded)
                        throw new InvalidOperationException(
                            string.Join(", ", result.Errors.Select(error => error.Description)));
                }

                if (!await userManager.IsInRoleAsync(user, seedUser.Role))
                {
                    var result = await userManager.AddToRoleAsync(user, seedUser.Role);
                    if (!result.Succeeded)
                        throw new InvalidOperationException(
                            string.Join(", ", result.Errors.Select(error => error.Description)));
                }
            }
        }
    }
}
