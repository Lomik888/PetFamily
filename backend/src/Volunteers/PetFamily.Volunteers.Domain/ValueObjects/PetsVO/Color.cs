using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

public class Color : ValueObject
{
    private const int VELUE_MIN_LENGHT = 1;
    public const int VELUE_MAX_LENGHT = 500;

    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Result<Color, Error> Create(string value)
    {
        var result = Validator.FieldValueObject.Validation(value, VELUE_MIN_LENGHT, VELUE_MAX_LENGHT);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new Color(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}