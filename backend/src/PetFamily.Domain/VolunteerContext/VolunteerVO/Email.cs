using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public class Email : ValueObject
{
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
        if (EmailRegex.IsMatch(value) == false)
        {
            return Error.Validation("Email is invalid", nameof(Email));
        }

        return new Email(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}