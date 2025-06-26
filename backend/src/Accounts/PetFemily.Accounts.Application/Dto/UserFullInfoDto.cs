using PetFamily.Core.Dtos;
using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Domain.ValueObjects;

namespace PetFemily.Accounts.Application.Dto;

public record UserFullInfoDto(
    Guid UserId,
    IEnumerable<SocialNetwork> SocialNetworks,
    string? Photo,
    string FullName,
    string Email,
    string UserName,
    string? PhoneNumber,
    IEnumerable<string> RoleName,
    IEnumerable<string> PermissionCodes,
    Guid? AdminAccountId,
    Guid? VolunteerAccountId,
    Guid? ParticipantAccountId,
    IEnumerable<Guid>? FavoritePetsIds,
    string? Certificates,
    IEnumerable<DetailsForHelp>? DetailsForHelp,
    int? Experience);