namespace WebApi.Middlewares;

public class AuditLogMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;

    public AuditLogMiddleware(RequestDelegate next, ILogger<AuditLogMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation($"Handling request: {context.Request.Method}.{context.Request.Path}");

        await next(context);

        logger.LogInformation($"Finished handling request: {context.Request.Method}.{context.Request.Path}, Response Status: {context.Response.StatusCode}");
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditLogMiddleware>();
    }
}