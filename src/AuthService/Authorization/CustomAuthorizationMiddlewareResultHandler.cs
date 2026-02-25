using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Text.Json;
using AuthService.Common;

namespace AuthService.Authorization;

public class CustomAuthorizationMiddlewareResultHandler
    : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler
        = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.FailResponse(
                "You must be an Admin to access this resource.");

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));

            return;
        }

        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}