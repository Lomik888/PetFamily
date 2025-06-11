using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Specieses.Application.Abstractions;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Domain.Ids;

namespace PetFamily.Specieses.Presentation.Contracts;

public class SpeciesExistenceContracts : ISpeciesExistenceContracts
{
    private readonly ISpeciesRepository _speciesRepository;

    public SpeciesExistenceContracts(ISpeciesRepository speciesRepository)
    {
        _speciesRepository = speciesRepository;
    }

    public async Task<Result<bool, ErrorList>> SpeciesAndBreedExistsAsync(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken)
    {
        var speciesIdResult = SpeciesId.Create(speciesId);
        if (speciesIdResult.IsFailure == true)
        {
            var error = speciesIdResult.Error;
            return ErrorList.Create(error);
        }

        var breedIdResult = BreedId.Create(breedId);
        if (breedIdResult.IsFailure == true)
        {
            var error = breedIdResult.Error;
            return ErrorList.Create(error);
        }

        var speciesIdVo = speciesIdResult.Value;
        var breedIdVo = breedIdResult.Value;

        var result = await _speciesRepository.SpeciesAndBreedExistsAsync(speciesIdVo, breedIdVo, cancellationToken);

        return result;
    }
}