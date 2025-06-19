using Microsoft.AspNetCore.Authorization;
using PetFamily.Framework;

namespace PetFamily.Accounts.Presentation;

public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (string.IsNullOrWhiteSpace(policyName))
        {
            await GetFallbackPolicyAsync();
        }

        var policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new HasPermission(policyName))
            .RequireAuthenticatedUser()
            .Build();

        return await Task.FromResult<AuthorizationPolicy?>(policy);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy>(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy?>(null);
}