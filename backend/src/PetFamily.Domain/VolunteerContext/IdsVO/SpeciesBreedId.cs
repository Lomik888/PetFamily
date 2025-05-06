using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.IdsVO;

public class SpeciesBreedId : ValueObject
{
    public Guid SpeciesId { get; }

    public Guid BreedId { get; }

    private SpeciesBreedId(Guid speciesId, Guid breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }

    public static Result<SpeciesBreedId, Error> Create(Guid speciesId, Guid breedId)
    {
        if (speciesId == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Species is required", nameof(speciesId));
        }

        if (breedId == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Breed is required", nameof(breedId));
        }

        return new SpeciesBreedId(speciesId, breedId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }
}