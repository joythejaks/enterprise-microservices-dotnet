using System.Net;
using System.Text.Json;
using AuthService.Common;

namespace AuthService.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.FailResponse(
                "An unexpected error occurred.",
                ex.Message);

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}