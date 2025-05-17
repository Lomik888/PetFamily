using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.Contracts.DTO.SharedDtos;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateDetailsForHelps;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record UpdateVolunteersDetailsForHelpRequest(
    IEnumerable<DetailsForHelpDto> DetailsForHelps
)
    : IToCommand<UpdateVolunteersDetailsForHelpCommand, Guid>
{
    public UpdateVolunteersDetailsForHelpCommand ToCommand(Guid volunteerId)
    {
        return new UpdateVolunteersDetailsForHelpCommand(
            volunteerId,
            new DetailsForHelpCollectionDto(DetailsForHelps.ToList()));
    }
}