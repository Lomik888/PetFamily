using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class Color : ValueObject
{
    private const int VELUE_MIN_LENGHT = 1;
    private const int VELUE_MAX_LENGHT = 500;

    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Result<Color, Error> Create(string value)
    {
        var result = FieldValidator.ValidationField(value, VELUE_MIN_LENGHT, VELUE_MAX_LENGHT);

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