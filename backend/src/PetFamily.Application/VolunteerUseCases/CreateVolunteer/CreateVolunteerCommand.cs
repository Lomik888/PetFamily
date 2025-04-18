using PetFamily.Application.DTO.SharedDtos;
using PetFamily.Application.DTO.VolunteerDtos;

namespace PetFamily.Application.VolunteerUseCases.CreateVolunteer;

public record CreateVolunteerCommand(
    NameDto Name,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
);