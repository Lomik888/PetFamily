using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.Commands.Create;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record CreateVolunteerRequest(
    VolunteerNameDto VolunteerName,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
) : IToCommand<CreateVolunteerCommand>
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            VolunteerName,
            Email,
            Description,
            Experience,
            Phone);
    }
}