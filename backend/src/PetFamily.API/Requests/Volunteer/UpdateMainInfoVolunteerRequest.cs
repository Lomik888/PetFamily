using PetFamily.API.Requests.Interfaces;
using PetFamily.Application.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.UpdateMainInfo;

namespace PetFamily.API.Requests.Volunteer;

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