using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.SharedKernel.ValueObjects;

public class Email : ValueObject
{
    private const int EMAIL_MIN_LENGTH = 1;
    public const int EMAIL_MAX_LENGTH = 50;

    private static readonly Regex EmailRegex = new Regex(
        REGULAR_Email,
        RegexOptions.Compiled |
        RegexOptions.IgnoreCase |
        RegexOptions.IgnorePatternWhitespace);

    private const string REGULAR_Email = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string value)
    {
        var result = Validator.FieldValueObject.Validation(value, EMAIL_MIN_LENGTH, EMAIL_MAX_LENGTH);

        if (result.IsFailure)
        {
            return ErrorsPreform.General.Validation("Email is invalid", nameof(Email));
        }

        if (EmailRegex.IsMatch(value) == false)
        {
            return ErrorsPreform.General.Validation("Email is invalid", nameof(Email));
        }

        return new Email(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}