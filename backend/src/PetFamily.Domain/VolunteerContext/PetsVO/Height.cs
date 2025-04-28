using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;


namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class Height : ValueObject
{
    private const double VELUE_MAX_LENGHT = 150D;
    private const int VALUE_DECIMAL_PRECISION = 2;
    private const bool VELUE_CAN_BE_NEGATIVE = false;
    public double Value { get; }

    private Height(double value)
    {
        Value = value;
    }

    public static Result<Height, Error> Create(double value)
    {
        value = Math.Round(value, VALUE_DECIMAL_PRECISION);

        var result = Validator.FieldValueObject.ValidationNumber<double>(
            value,
            VELUE_CAN_BE_NEGATIVE,
            VALUE_DECIMAL_PRECISION,
            VELUE_MAX_LENGHT);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new Height(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}