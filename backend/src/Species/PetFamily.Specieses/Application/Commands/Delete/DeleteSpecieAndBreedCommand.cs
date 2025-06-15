using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Specieses.Application.Commands.Delete;

public record DeleteSpecieAndBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;