namespace ClaimDesk.Api.Infrastructure;

public class DeveloperExceptionResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DeveloperExceptionResponseMiddleware> _logger;

    public DeveloperExceptionResponseMiddleware(RequestDelegate next, ILogger<DeveloperExceptionResponseMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request failed for {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Request failed.",
                detail = ex.Message,
                exceptionType = ex.GetType().Name
            });
        }
    }
}
