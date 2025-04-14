using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.SpeciesContext.SharedVO;

public class Name : ValueObject
{
    private const int VELUE_MIN_LENGHT = 1;
    public const int VELUE_MAX_LENGHT = 30;

    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name, Error> Create(string value)
    {
        var result = FieldValidator.ValidationField(value, VELUE_MIN_LENGHT, VELUE_MAX_LENGHT);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new Name(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}