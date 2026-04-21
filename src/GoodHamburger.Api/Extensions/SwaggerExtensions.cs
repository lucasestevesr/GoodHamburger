using GoodHamburger.Application;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace GoodHamburger.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GoodHamburger API",
                    Version = "v1",
                    Description = "API para gerenciamento de pedidos do GoodHamburger."
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Informe o token JWT no formato: Bearer {token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.SupportNonNullableReferenceTypes();

                foreach (var xmlPath in GetXmlDocumentationPaths())
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }
            });

            return services;
        }

        public static WebApplication UseSwaggerDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodHamburger API v1");
                options.DocumentTitle = "GoodHamburger API";
                options.DisplayRequestDuration();
            });

            return app;
        }

        private static IEnumerable<string> GetXmlDocumentationPaths()
        {
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(ApplicationAssemblyReference).Assembly
            };

            return assemblies
                .Select(assembly => Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml"))
                .Where(File.Exists)
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }
}
