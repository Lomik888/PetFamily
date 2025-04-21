using PetFamily.Application.DTO.VolunteerDtos;
using PetFamily.Application.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Create;

public record CreateVolunteerCommand(
    NameCreateDto Name,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
) : ICommand;