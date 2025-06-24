using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using PetFamily.Core.Dtos;
using PetFemily.Accounts.Domain.ValueObjects;

namespace PetFemily.Accounts.Application.Dto;

public class UserDto : IdentityUser<Guid>
{
    public Guid Id { get; init; }
    public List<SocialNetworkDto> SocialNetworks { get; init; } = default!;
    public string? Photo { get; init; }
    public string FullName { get; init; } = default!;
    public string Email { get; init; } = default!;

    public AdminAccountDto? AdminAccount { get; init; }

    public VolunteerAccountDto? VolunteerAccount { get; init; }

    public ParticipantAccountDto? ParticipantAccount { get; init; }

    public List<RoleDto> Roles { get; init; }
}