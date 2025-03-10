using System.Diagnostics;

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
    public async Task InvokeAsync(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next.Invoke(c);
        logger.LogDebug($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("userName")?.Value ?? "unknown"}");
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditLogMiddleware>();
    }
}
