using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record UpdateMainInfoVolunteerRequest(
    NameUpdateDto? Name,
    string? Description,
    int? Experience
) : IToCommand<UpdateMainInfoVolunteerCommand, Guid>
{
    public UpdateMainInfoVolunteerCommand ToCommand(Guid volunteerId)
    {
        return new UpdateMainInfoVolunteerCommand(
            volunteerId,
            Name,
            Description,
            Experience
        );
    }
}