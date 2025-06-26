using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteersRequests.Domain.Enums;
using PetFamily.VolunteersRequests.Domain.ValueObjects;
using PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

namespace PetFamily.VolunteersRequests.Domain.Dtos;

public record CreateVolunteerRequestDto(
    AdminId AdminId,
    UserId UserId,
    VolunteerInfo VolunteerInfo);