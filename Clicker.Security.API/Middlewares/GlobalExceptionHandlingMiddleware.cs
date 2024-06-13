using Clicker.Security.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
namespace Clicker.Security.API.Middlewares;


public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled exception has occurred");

            var problemDetails = e switch
            {
                AuthException apiException => new ProblemDetails
                {
                    Status = apiException.StatusCode,
                    Title = "Auth exception occurred",
                    Detail = apiException.Message
                },
                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Server error",
                    Detail = e.Message
                }
            };

            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}

