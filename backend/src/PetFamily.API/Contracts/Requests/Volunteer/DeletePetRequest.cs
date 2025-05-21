using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.Delete;
using PetFamily.Application.VolunteerUseCases.Commands.DeletePet;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record DeletePetRequest(DeleteType DeleteType) : IToCommand<DeletePetCommand, Guid, Guid>
{
    public DeletePetCommand ToCommand(Guid volunterId, Guid petId)
    {
        return new DeletePetCommand(volunterId, petId, DeleteType);
    }
}