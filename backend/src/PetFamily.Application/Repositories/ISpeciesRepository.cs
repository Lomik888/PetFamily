using PetFamily.Domain.SpeciesContext.Ids;

namespace PetFamily.Application.Repositories;

public interface ISpeciesRepository
{
    Task<bool> SpeciesAndBreedExistsAsync(
        SpeciesId speciesId,
        BreedId breedId,
        CancellationToken cancellationToken = default);
}