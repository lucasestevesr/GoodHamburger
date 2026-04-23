using GoodHamburger.Application.Identity.Interfaces;
using GoodHamburger.Application.Orders.Interfaces;
using GoodHamburger.Application.Products.Interfaces;
using GoodHamburger.Infra.Data;
using GoodHamburger.Infra.Data.Repositories;
using GoodHamburger.Infra.Identity;
using GoodHamburger.Infra.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.CrossCutting.IoC
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'Default' não configurada.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));

            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 7;
                    options.Password.RequireNonAlphanumeric = true;
                    options.User.RequireUniqueEmail = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
