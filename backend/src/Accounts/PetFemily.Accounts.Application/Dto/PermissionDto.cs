using System.Text.Json.Serialization;

namespace PetFemily.Accounts.Application.Dto;

public class PermissionDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = default!;

    public List<RoleDto> Roles { get; init; } = [];
}