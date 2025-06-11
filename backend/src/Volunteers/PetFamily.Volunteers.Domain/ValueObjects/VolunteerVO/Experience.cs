using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

public class Experience : ValueObject
{
    public const int VELUE_MAX_LENGHT = 150;
    private const bool VELUE_CAN_BE_NEGATIVE = false;
    public int Value { get; }

    private Experience(int value)
    {
        Value = value;
    }

    public static Result<Experience, Error> Create(int value)
    {
        var result = Validator.FieldValueObject.ValidationNumber<int>(
            value,
            VELUE_CAN_BE_NEGATIVE,
            null,
            VELUE_MAX_LENGHT);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new Experience(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}