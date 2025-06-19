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

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermission requirement)
    {
        using var scope = _scopeFactory.CreateScope();
        var accountDbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();

        var roleName = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Typ);
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

        var userHavePermission = await accountDbContext.Permissions
            .AnyAsync(x => x.Code == policy && x.Roles.Any(x => x.Name == roleName.Value));

        if (userHavePermission == false)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}