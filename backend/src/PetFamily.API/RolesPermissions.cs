namespace PetFamily.API;

public class RolesPermissions
{
    public Dictionary<string, List<string>> Roles { get; set; } = null!;
    public Dictionary<string, List<string>> Permissions { get; set; } = null!;
}