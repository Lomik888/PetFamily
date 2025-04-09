using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.SharedVO;

public class Description : ValueObject
{
    private const int VALUE_MIN_LENGHT = 1;
    private const int VALUE_MAX_LENGHT = 500;

    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        var result = FieldValidator.ValidationField(value, VALUE_MIN_LENGHT, VALUE_MAX_LENGHT);

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