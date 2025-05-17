using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Commands.Create;

public record CreateVolunteerCommand(
    NameCreateDto Name,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
) : ICommand;