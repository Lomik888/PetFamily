using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Specieses.Contracts;

public interface ISpeciesExistenceContracts
{
    Task<Result<bool, ErrorList>> SpeciesAndBreedExistsAsync(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken);
}