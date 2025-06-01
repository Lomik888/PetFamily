using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class Age : ValueObject
{
    public const uint VELUE_MAX = 30;

    public uint Value { get; }

    private Age(uint value)
    {
        Value = value;
    }

    public static Result<Age, Error> Create(uint value)
    {
        if (value > VELUE_MAX)
        {
            return ErrorsPreform.General.Validation("Age must be < 30", nameof(value));
        }

        return new Age(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}