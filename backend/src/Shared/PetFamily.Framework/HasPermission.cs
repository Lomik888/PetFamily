using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Framework;

public class HasPermission : AuthorizeAttribute, IAuthorizationRequirement
{
    public HasPermission(string permissionCode) : base(policy: permissionCode)
    {
    }
}