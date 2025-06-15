using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

namespace PetFamily.Volunteers.Application.Commands.Create;

public record CreateVolunteerCommand(
    VolunteerNameDto VolunteerName,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
) : ICommand;