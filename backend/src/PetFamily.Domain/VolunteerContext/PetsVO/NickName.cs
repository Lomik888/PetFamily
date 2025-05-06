using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class NickName : ValueObject
{
    public const int VELUE_MIN_LENGHT = 1;
    public const int VELUE_MAX_LENGHT = 30;

    public string Value { get; }

    private NickName(string value)
    {
        Value = value;
    }

    public static Result<NickName, Error> Create(string value)
    {
        var result = Validator.FieldValueObject.Validation(value, VELUE_MIN_LENGHT, VELUE_MAX_LENGHT);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return new NickName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}