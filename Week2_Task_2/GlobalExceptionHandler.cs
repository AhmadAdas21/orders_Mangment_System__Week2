using Microsoft.AspNetCore.Diagnostics;
using Week2_Task_2.Dto;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "An unexpected error occurred while processing the request"
        );

        int statusCode;

        if (exception is InvalidOperationException)
        {
            statusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new error_response
        {
            statusCode = statusCode,
            message = exception.Message
        };

        await context.Response.WriteAsJsonAsync(
            response,
            cancellationToken
        );

        return true;
    }
}