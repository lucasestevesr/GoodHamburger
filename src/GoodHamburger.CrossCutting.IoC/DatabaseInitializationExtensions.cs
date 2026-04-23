using GoodHamburger.Infra.Data;
using GoodHamburger.Infra.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.CrossCutting.IoC
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task ApplyDevelopmentMigrationsAndSeedAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            await provider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
            await provider.GetRequiredService<IdentityDbContext>().Database.MigrateAsync();

            await IdentitySeeds.SeedRoles(provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>());
            await IdentitySeeds.SeedUsers(
                provider.GetRequiredService<UserManager<ApplicationUser>>(),
                provider.GetRequiredService<IConfiguration>());
        }
    }
}
