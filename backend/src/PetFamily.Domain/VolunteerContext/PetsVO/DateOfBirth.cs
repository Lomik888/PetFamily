using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class DateOfBirth : ValueObject
{
    private static readonly DateTime VALUE_DATE_AFTER_INVALID = new DateTime(2000, 1, 26);

    public DateTime Value { get; }

    private DateOfBirth(DateTime value)
    {
        Value = value;
    }

    public static Result<DateOfBirth, Error> Create(DateTime value)
    {
        if (value > DateTime.UtcNow || value < VALUE_DATE_AFTER_INVALID)
        {
            return Error.Validation("Date of Birth is invalid", nameof(CreatedAt));
        }

        return new DateOfBirth(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}