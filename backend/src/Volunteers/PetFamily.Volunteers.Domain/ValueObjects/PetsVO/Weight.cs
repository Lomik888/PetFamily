using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

public class Weight : ValueObject
{
    public const double VELUE_MAX_LENGHT = 200D;
    private const int VALUE_DECIMAL_PRECISION = 2;
    private const bool VALUE_CAN_BE_NEGATIVE = false;

    public double Value { get; }

    private Weight(double value)
    {
        Value = value;
    }

    public static Result<Weight, Error> Create(double value)
    {
        value = Math.Round(value, VALUE_DECIMAL_PRECISION);

        var result = Validator.FieldValueObject.ValidationNumber<double>(
            value,
            VALUE_CAN_BE_NEGATIVE,
            VALUE_DECIMAL_PRECISION,
            VELUE_MAX_LENGHT);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new Weight(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}