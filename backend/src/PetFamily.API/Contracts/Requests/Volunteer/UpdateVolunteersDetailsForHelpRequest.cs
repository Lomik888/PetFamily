using PetFamily.API.Requests.Interfaces;
using PetFamily.Application.Contracts.DTO.SharedDtos;
using PetFamily.Application.VolunteerUseCases.UpdateDetailsForHelps;

namespace PetFamily.API.Requests.Volunteer;

public record UpdateVolunteersDetailsForHelpRequest(
    IReadOnlyList<DetailsForHelpDto> DetailsForHelps
)
    : IToCommand<UpdateVolunteersDetailsForHelpCommand, Guid>
{
    public UpdateVolunteersDetailsForHelpCommand ToCommand(Guid volunteerId)
    {
        return new UpdateVolunteersDetailsForHelpCommand(
            volunteerId,
            new DetailsForHelpCollectionDto(DetailsForHelps));
    }
}