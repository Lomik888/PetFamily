using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateMainInfo;

namespace PetFamily.API.Contracts.Requests.Volunteer;

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