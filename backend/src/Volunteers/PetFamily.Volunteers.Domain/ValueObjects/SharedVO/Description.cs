using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Volunteers.Domain.ValueObjects.SharedVO;

public class Description : ValueObject
{
    private const int VALUE_MIN_LENGHT = 1;
    public const int VALUE_MAX_LENGHT = 500;

    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        var result = Validator.FieldValueObject.Validation(value, VALUE_MIN_LENGHT, VALUE_MAX_LENGHT);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new Description(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}