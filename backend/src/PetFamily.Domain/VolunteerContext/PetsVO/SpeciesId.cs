using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class SpeciesId : ValueObject
{
    public Guid Value { get; }

    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public static Result<SpeciesId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Error.Validation("Species is required", nameof(SpeciesId));
        }

        return new SpeciesId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}