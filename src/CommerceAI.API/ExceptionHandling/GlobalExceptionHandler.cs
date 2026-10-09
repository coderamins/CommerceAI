using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommerceAI.API.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
                exception,
                "Unhandled exception occurred.");

        var problemDetails = exception switch
        {
            ValidationException validationException =>
                CreateValidationProblemDetails(
                    httpContext,
                    validationException),

            KeyNotFoundException notFoundException =>
                CreateNotFoundProblemDetails(
                    httpContext,
                    notFoundException),

            DbUpdateConcurrencyException =>
                CreateConcurrencyProblemDetails(httpContext),

            _ => CreateInternalServerError(httpContext)
        };

        httpContext.Response.StatusCode =
            problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

        return true;
    }

    private static ProblemDetails CreateInternalServerError(
      HttpContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = "An unexpected error occurred while processing the request.",
            Instance = context.Request.Path
        };
    }

    private static ValidationProblemDetails
         CreateValidationProblemDetails(
             HttpContext context,
             ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(x => x.ErrorMessage)
                    .ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed.",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateConcurrencyProblemDetails(
        HttpContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Concurrency conflict.",
            Detail = "The resource was modified by another request. Please reload the resource and try again.",
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateNotFoundProblemDetails(
    HttpContext context,
    KeyNotFoundException exception)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Resource not found.",
            Detail = exception.Message,
            Instance = context.Request.Path
        };
    }
}
