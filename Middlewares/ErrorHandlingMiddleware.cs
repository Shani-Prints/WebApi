namespace WebApi.Middlewares;

public class ErrorHandlingMiddleware
{
    private RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 500;

        var response = new
        {
            message = "An unexpected error occurred.",
            details = exception.Message
        };

        return httpContext.Response.WriteAsJsonAsync(response);
    }
}

public static partial class MiddlewareExtensionsF
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }

}

