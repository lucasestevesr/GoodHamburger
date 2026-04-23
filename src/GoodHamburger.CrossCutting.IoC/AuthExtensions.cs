using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Infra.Identity.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.CrossCutting.IoC
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(options =>
            {
                var section = configuration.GetSection(JwtOptions.SectionName);

                options.Issuer = section["Issuer"] ?? string.Empty;
                options.Audience = section["Audience"] ?? string.Empty;
                options.SecretKey = section["SecretKey"] ?? string.Empty;
                options.ExpiresInMinutes = int.TryParse(section["ExpiresInMinutes"], out var expiresInMinutes)
                    ? expiresInMinutes
                    : 60;
            });

            services.AddScoped<IAccessTokenService, JwtAccessTokenService>();

            return services;
        }
    }
}
