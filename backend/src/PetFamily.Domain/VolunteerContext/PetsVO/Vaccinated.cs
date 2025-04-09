using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class Vaccinated : ValueObject
{
    public bool Value { get; }

    private Vaccinated(bool value)
    {
        Value = value;
    }

    public static Result<Vaccinated, Error> Create(bool value)
    {
        return new Vaccinated(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}