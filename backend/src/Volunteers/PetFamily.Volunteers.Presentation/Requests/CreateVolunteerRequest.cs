using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

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