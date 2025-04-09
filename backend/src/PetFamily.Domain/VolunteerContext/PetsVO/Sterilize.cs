using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class Sterilize : ValueObject
{
    public bool Value { get; }

    private Sterilize(bool value)
    {
        Value = value;
    }

    public static Result<Sterilize, Error> Create(bool value)
    {
        return new Sterilize(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}