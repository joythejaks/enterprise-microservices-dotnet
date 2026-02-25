using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthService.Authorization;

public class AdminHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminRequirement requirement)
    {
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "Admin")
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}