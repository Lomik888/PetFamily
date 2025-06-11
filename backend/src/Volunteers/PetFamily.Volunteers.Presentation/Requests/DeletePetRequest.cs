using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.Delete;
using PetFamily.Volunteers.Application.Commands.DeletePet;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record DeletePetRequest(DeleteType DeleteType) : IToCommand<DeletePetCommand, Guid, Guid>
{
    public DeletePetCommand ToCommand(Guid volunterId, Guid petId)
    {
        return new DeletePetCommand(volunterId, petId, DeleteType);
    }
}