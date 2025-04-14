using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class BreedId : ValueObject
{
    public Guid Value { get; }

    private BreedId(Guid value)
    {
        Value = value;
    }

    public static Result<BreedId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Breed is required", nameof(BreedId));
        }

        return new BreedId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}