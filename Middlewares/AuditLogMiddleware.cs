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
        // logger.LogInformation($"Handling request: {context.Request.Method}.{context.Request.Path}");

        // await next(context);

        // logger.LogInformation($"Finished handling request: {context.Request.Method}.{context.Request.Path}, Response Status: {context.Response.StatusCode}");
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


// namespace WebApi.Middlewares
// {
//     public class AuditLogMiddleware
//     {
//         private readonly RequestDelegate next;
//         private readonly ILogger logger;

//         public AuditLogMiddleware(RequestDelegate next, ILogger<AuditLogMiddleware> logger)
//         {
//             this.next = next;
//             this.logger = logger;
//         }

//         public async Task InvokeAsync(HttpContext context)
//         {
//             // בדיקה אם יש משתמש מחובר
//             var user = context.User.Identity?.IsAuthenticated == true
//                 ? context.User.Identity.Name 
//                 : "לא מחובר";

//             // הוספת לוג של המידע על המשתמש
//             logger.LogInformation($"Handling request: {context.Request.Method}.{context.Request.Path}, User: {user}");

//             await next(context);

//             logger.LogInformation($"Finished handling request: {context.Request.Method}.{context.Request.Path}, Response Status: {context.Response.StatusCode}, User: {user}");
//         }
//     }

//     public static partial class MiddlewareExtensions
//     {
//         public static IApplicationBuilder UseAuditLogMiddleware(this IApplicationBuilder builder)
//         {
//             return builder.UseMiddleware<AuditLogMiddleware>();
//         }
//     }
// }
