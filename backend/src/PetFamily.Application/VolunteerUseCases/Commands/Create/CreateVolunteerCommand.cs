using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Commands.Create;

public record CreateVolunteerCommand(
    VolunteerNameDto VolunteerName,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
) : ICommand;