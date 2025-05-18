using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.SpeciesUseCases.Commands.Delete;

public record DeleteSpecieAndBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;