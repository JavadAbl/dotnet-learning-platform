using Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        // 1. Handle Known Application Exceptions (4xx)
        if (exception is AppException appException)
        {
            // Log as Warning, not Error, because 4xx are expected client/business errors
            _logger.LogWarning(appException,
                "Handled application exception [{ErrorCode}]: {Message}",
                appException.ErrorCode, appException.Message);

            httpContext.Response.StatusCode = appException.StatusCode;

            problemDetails.Status = appException.StatusCode;
            problemDetails.Title = appException.ErrorCode;
            problemDetails.Detail = appException.Message;

            // Add the custom error code to the extensions
            problemDetails.Extensions["errorCode"] = appException.ErrorCode;
        }
        // 2. Handle Unknown System Exceptions (5xx)
        else
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "An unexpected error occurred";

            // NEVER expose internal exception details in Production
            problemDetails.Detail = httpContext.RequestServices
                .GetRequiredService<IHostEnvironment>().IsDevelopment()
                    ? exception.Message
                    : "Please contact support with the traceId.";

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    // Helper to map status codes to standard RFC 7807 Titles
    private static string GetTitleForStatusCode(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        _ => "Error"
    };
}