using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.SpeciesUseCases.Commands.Delete;

namespace PetFamily.API.Contracts.Requests.Species;

public record DeleteSpecieAndBreedRequest(Guid BreedId) : IToCommand<DeleteSpecieAndBreedCommand, Guid>
{
    public DeleteSpecieAndBreedCommand ToCommand(Guid speciesId)
    {
        return new DeleteSpecieAndBreedCommand(speciesId, BreedId);
    }
}