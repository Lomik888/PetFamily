using Microsoft.AspNetCore.Identity;

namespace PetFemily.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public new string Name { get; set; } = default!;
    public List<Permission> Permissions { get; set; } = [];
    public List<User> Users { get; set; } = [];
}