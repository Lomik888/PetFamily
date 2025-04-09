using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.SharedVO;

public class PhoneNumber : ValueObject
{
    private static readonly Regex PhoneNumberRegex = new Regex(
        VELUE_REGULAR_PHONENUMBER,
        RegexOptions.Compiled |
        RegexOptions.IgnoreCase |
        RegexOptions.IgnorePatternWhitespace);

    private const string VELUE_REGULAR_PHONENUMBER = @"^\+?[1-9]\d{1,14}$";

    public string RegionCode { get; }
    public string Number { get; }
    public string FullNumber => $"{RegionCode}{Number}";

    private PhoneNumber(string regionCode, string number)
    {
        RegionCode = regionCode;
        Number = number;
    }

    public static Result<PhoneNumber, Error> Create(string regionCode, string number)
    {
        var fullNumber = $"{regionCode}{number}";

        if (PhoneNumberRegex.IsMatch(fullNumber) == false)
        {
            return Error.Validation("Phone number is invalid", nameof(PhoneNumber));
        }

        return new PhoneNumber(regionCode, number);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RegionCode;
        yield return Number;
    }
}