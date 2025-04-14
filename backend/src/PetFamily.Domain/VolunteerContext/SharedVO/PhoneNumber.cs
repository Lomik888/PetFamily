using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.SharedVO;

public class PhoneNumber : ValueObject
{
    public const int PHONENUMBER_MIN_LENGTH = 2;
    public const int PHONENUMBER_MAX_LENGTH = 16;
    public const int NUMBER_MAX_LENGTH = 14;

    private static readonly Regex PhoneNumberRegex = new Regex(
        VELUE_REGULAR_PHONENUMBER,
        RegexOptions.Compiled |
        RegexOptions.IgnoreCase |
        RegexOptions.IgnorePatternWhitespace);

    private const string VELUE_REGULAR_PHONENUMBER = @"^\+?[1-9]\d{1,14}$";

    public string RegionCode { get; }
    public string Number { get; }
    public string FullNumber() => $"{RegionCode}{Number}";

    private PhoneNumber(string regionCode, string number)
    {
        RegionCode = regionCode;
        Number = number;
    }

    public static Result<PhoneNumber, Error> Create(string regionCode, string number)
    {
        var fullNumber = $"{regionCode}{number}";

        var result = FieldValidator.ValidationField(fullNumber, PHONENUMBER_MIN_LENGTH, PHONENUMBER_MAX_LENGTH);

        if (result.IsFailure)
        {
            return ErrorsPreform.General.Validation("Phone number is invalid", nameof(PhoneNumber));
        }

        if (PhoneNumberRegex.IsMatch(fullNumber) == false)
        {
            return ErrorsPreform.General.Validation("Phone number is invalid", nameof(PhoneNumber));
        }

        return new PhoneNumber(regionCode, number);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RegionCode;
        yield return Number;
    }
}