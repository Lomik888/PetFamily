using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation;

public class PermissionAuthorizationHandler : AuthorizationHandler<HasPermission>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasPermission requirement)
    {
        using var scope = _scopeFactory.CreateScope();
        var accountDbContext = scope.ServiceProvider.GetRequiredService<WriteAccountDbContext>();

        var roleName = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        if (roleName is null)
        {
            context.Fail();
            return;
        }

        var policy = requirement.Policy;
        if (policy is null)
        {
            context.Succeed(requirement);
            return;
        }

        var userHavePermission = await accountDbContext.Roles.Where(x => x.Name == roleName.Value)
            .AnyAsync(x => x.Permissions.Any(x => x.Code == policy));
        if (userHavePermission == false)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}