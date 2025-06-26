using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Discussions.Domain.ValueObjects;

public class Text : ValueObject
{
    public const int MIN_LENGTH = 1;
    public const int MAX_LENGTH = 1000;

    public string Value { get; }

    private Text(string value)
    {
        Value = value;
    }

    public static Result<Text, ErrorList> Create(string value)
    {
        var validationResult = Validator.FieldValueObject.Validation(value, MIN_LENGTH, MAX_LENGTH);
        if (validationResult.IsFailure == true)
        {
            var error = validationResult.Error;
            return ErrorList.Create(error);
        }

        return new Text(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}