using PetFamily.Core.Dtos;
using PetFamily.VolunteersRequests.Domain.Enums;

namespace PetFamily.Data.Tests.Requests;

public record RequestVolunteerRequest(
    Guid AdminId,
    Guid UserId,
    string? Certificates,
    int Experience,
    IEnumerable<DetailsForHelpDto> DetailsForHelps);