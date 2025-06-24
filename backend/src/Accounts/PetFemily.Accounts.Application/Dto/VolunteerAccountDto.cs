using PetFamily.Core.Dtos;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFemily.Accounts.Application.Dto;

public class VolunteerAccountDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string? Certificates { get; init; }
    public List<DetailsForHelpDto> DetailsForHelps { get; init; }
    public int Experience { get; init; }
}