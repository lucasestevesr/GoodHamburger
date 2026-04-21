using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddApiValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Requisição inválida.",
                        Detail = "Confira os campos enviados e tente novamente.",
                        Instance = context.HttpContext.Request.Path
                    };

                    problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                    return new BadRequestObjectResult(problemDetails);
                };
            });

            return services;
        }
    }
}
