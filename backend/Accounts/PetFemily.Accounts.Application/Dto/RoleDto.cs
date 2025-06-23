using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace PetFemily.Accounts.Application.Dto;

public class RoleDto : IdentityRole<Guid>
{
    public new string Name { get; set; } = default!;
    [JsonIgnore]
    public List<PermissionDto> Permissions { get; set; } = [];
    [JsonIgnore]
    public List<UserDto> Users { get; set; } = [];
}