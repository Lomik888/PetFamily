using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.UpdateStatusPet;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record UpdateStatusPetRequest(HelpStatuses HelpStatus) : IToCommand<UpdateStatusPetCommand, Guid, Guid>
{
    public UpdateStatusPetCommand ToCommand(Guid volunterId, Guid petId)
    {
        return new UpdateStatusPetCommand(volunterId, petId, HelpStatus);
    }
}