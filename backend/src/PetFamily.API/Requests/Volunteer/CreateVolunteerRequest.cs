using PetFamily.API.Requests.Interfaces;
using PetFamily.Application.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.Create;

namespace PetFamily.API.Requests.Volunteer;

public record CreateVolunteerRequest(
    NameCreateDto Name,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
) : IToCommand<CreateVolunteerCommand>
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            Name,
            Email,
            Description,
            Experience,
            Phone);
    }
}