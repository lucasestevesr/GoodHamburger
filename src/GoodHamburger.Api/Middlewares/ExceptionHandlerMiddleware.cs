using GoodHamburger.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Middlewares
{
    public sealed class ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Erro ao processar a requisição.");
                await WriteErrorResponseAsync(context, exception);
            }
        }

        private static async Task WriteErrorResponseAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title) = exception switch
            {
                ArgumentException => (
                    StatusCodes.Status400BadRequest,
                    "Requisição inválida."),
                DomainException => (
                    StatusCodes.Status400BadRequest,
                    "Regra de negócio inválida."),
                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    "Recurso não encontrado."),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Erro interno do servidor.")
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path,
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
