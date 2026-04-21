using System.Security.Claims;
using System.Text;
using GoodHamburger.Api.Security;
using GoodHamburger.Domain.Entities.Auth;
using GoodHamburger.Infra.Data.Security;
using GoodHamburger.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GoodHamburger.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("Configuração JWT não encontrada.");

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, HttpContextCurrentUser>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.OrderManagement,
                    policy => policy.RequireRole(
                        nameof(UserRole.Attendant),
                        nameof(UserRole.Manager),
                        nameof(UserRole.Admin)));

                options.AddPolicy(
                    AuthorizationPolicies.ProductManagement,
                    policy => policy.RequireRole(
                        nameof(UserRole.Manager),
                        nameof(UserRole.Admin)));

                options.AddPolicy(
                    AuthorizationPolicies.CreateAttendantManagement,
                    policy => policy.RequireRole(
                        nameof(UserRole.Manager),
                        nameof(UserRole.Admin)));

                options.AddPolicy(
                    AuthorizationPolicies.UserManagement,
                    policy => policy.RequireRole(nameof(UserRole.Admin)));
            });

            return services;
        }
    }
}
