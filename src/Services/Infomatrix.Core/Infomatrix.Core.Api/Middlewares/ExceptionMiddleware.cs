using Infomatrix.Core.Application.Exceptions;
using Infomatrix.Core.Domain.Common;

namespace Infomatrix.Core.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Application.Exceptions.AuthenticationException ex)
        {
            _logger.LogWarning(
                ex,
                "Authentication failed: {Message}",
                ex.Message);

            var statusCode = GetAuthenticationStatusCode(ex.Message);
            context.Response.StatusCode = statusCode;
            await context.Response
                .WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(
                ex,
                "Domain validation failed: {Message}",
                ex.Message);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response
                .WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred.");

            if (context.Response.HasStarted) return;

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response
                .WriteAsJsonAsync(new { error = "Internal server error" });
        }
    }

    private static int GetAuthenticationStatusCode(string message)
    {
        return message switch
        {
            var msg when msg.Contains("not found", StringComparison.OrdinalIgnoreCase) 
                => StatusCodes.Status404NotFound,
            var msg when msg.Contains("already exists", StringComparison.OrdinalIgnoreCase) 
                => StatusCodes.Status409Conflict,
            var msg when msg.Contains("unknown", StringComparison.OrdinalIgnoreCase) 
                => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status401Unauthorized
        };
    }
}
