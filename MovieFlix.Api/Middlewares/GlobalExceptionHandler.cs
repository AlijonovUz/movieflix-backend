using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MovieFlix.Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage))),
            
            BadHttpRequestException badHttpRequestException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                badHttpRequestException.Message),
            
            ApplicationException applicationException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                applicationException.Message),
            
            KeyNotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFoundException.Message),
            
            DbUpdateException dbUpdateException => (
                StatusCodes.Status409Conflict,
                "Conflict",
                dbUpdateException.Message),

            UnauthorizedAccessException unauthorizedException => (
                StatusCodes.Status403Forbidden,
                "Forbidden",
                unauthorizedException.Message),

            InvalidOperationException invalidOperationException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                invalidOperationException.Message),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                exception.Message) // in dev, let's also see the message!
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
