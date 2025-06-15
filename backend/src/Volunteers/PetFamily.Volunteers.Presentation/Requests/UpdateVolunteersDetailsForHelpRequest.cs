using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.UpdateDetailsForHelps;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;

namespace PetFamily.Volunteers.Presentation.Requests;

public record UpdateVolunteersDetailsForHelpRequest(IEnumerable<DetailsForHelpDto> DetailsForHelps)
    : IToCommand<UpdateVolunteersDetailsForHelpCommand, Guid>
{
    public UpdateVolunteersDetailsForHelpCommand ToCommand(Guid volunteerId)
    {
        return new UpdateVolunteersDetailsForHelpCommand(
            volunteerId,
            new DetailsForHelpCollectionDto(DetailsForHelps.ToList()));
    }
}