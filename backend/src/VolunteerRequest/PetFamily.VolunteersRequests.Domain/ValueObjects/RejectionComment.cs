using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects;

public class RejectionComment : ValueObject
{
    public const int MIN_LENGTH = 10;
    public const int MAX_LENGTH = 1000;

    public string Value { get; }

    private RejectionComment(string value)
    {
        Value = value;
    }

    public static Result<RejectionComment, Error> Create(string value)
    {
        var validationResult = Validator.FieldValueObject.Validation(value, MIN_LENGTH, MAX_LENGTH);

        if (validationResult.IsFailure == true)
        {
            return validationResult.Error;
        }

        return new RejectionComment(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}