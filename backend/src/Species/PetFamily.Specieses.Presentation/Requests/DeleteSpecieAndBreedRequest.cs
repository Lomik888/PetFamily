using PetFamily.Framework.Abstractions;
using PetFamily.Specieses.Application.Commands.Delete;

namespace PetFamily.Specieses.Presentation.Requests;

public record DeleteSpecieAndBreedRequest(Guid BreedId) : IToCommand<DeleteSpecieAndBreedCommand, Guid>
{
    public DeleteSpecieAndBreedCommand ToCommand(Guid speciesId)
    {
        return new DeleteSpecieAndBreedCommand(speciesId, BreedId);
    }
}