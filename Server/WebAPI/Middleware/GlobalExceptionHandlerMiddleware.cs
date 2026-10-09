using Microsoft.AspNetCore.Http;

namespace WebAPI.Middleware;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(
                "An unexpected server error occurred.");
        }
    }
}