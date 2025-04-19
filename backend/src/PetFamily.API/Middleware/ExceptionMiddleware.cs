using PetFamily.API.Extensions;
using PetFamily.API.Response.Envelope;
using PetFamily.Shared.Errors;

namespace PetFamily.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            await Handler(e, context);
        }
    }

    private async Task Handler(Exception exception, HttpContext context)
    {
        _logger.LogCritical("Iternal server {0}", exception.Message);

        var error = ErrorsPreform.General.IternalServerError(exception.Message).ToErrorResponse();

        var envelope = Envelope.Error(error);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(envelope);
    }
}