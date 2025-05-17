using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.Commands.Create;

namespace PetFamily.API.Contracts.Requests.Volunteer;

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