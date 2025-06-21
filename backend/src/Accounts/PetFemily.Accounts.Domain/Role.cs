using Microsoft.AspNetCore.Identity;

namespace PetFemily.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public new string Name { get; set; } = default!;
    public List<Permission> Permissions { get; set; } = [];
    public List<User> Users { get; set; } = [];

    public override int GetHashCode()
    {
        var nameHash = Name.GetHashCode();
        var permissionsHash = Permissions
            .OrderBy(x => x.Code)
            .Select(x => x.Code.GetHashCode())
            .Aggregate(0, (current, hash) => current ^ hash);

        var roleHash = nameHash ^ permissionsHash;

        return roleHash;
    }
}