using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.SharedKernel.ValueObjects;

public class CreatedAt : ValueObject
{
    private readonly DateTime _validateDate =
        new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public DateTime Value { get; }

    private CreatedAt(DateTime value)
    {
        Value = value;
    }

    public Result<CreatedAt, Error> Create(DateTime value)
    {
        var validateResult = IsValid(value);
        if (validateResult == false)
        {
            var error = ErrorsPreform.General.Validation("Date value is invalid", nameof(CreatedAt));
            return error;
        }

        return new CreatedAt(value);
    }

    protected virtual bool IsValid(DateTime value)
    {
        var validateDate = _validateDate;
        var result = value < validateDate;
        return result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}