namespace PetFemily.Accounts.Domain;

public class Permission
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public List<Role> Roles { get; set; } = [];
}