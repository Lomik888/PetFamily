using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class SerialNumber : ValueObject
{
    public const int INITIAL_VALUE = 1;
    public uint Value { get; }

    public SerialNumber(uint value)
    {
        Value = value;
    }

    public static Result<SerialNumber, Error> Create(uint value)
    {
        if (value < INITIAL_VALUE)
        {
            return ErrorsPreform.General.Validation("Serial number must be greater than zero", nameof(value));
        }

        return new SerialNumber(value);
    }

    public static SerialNumber CreateFirst()
    {
        return new SerialNumber(1);
    }

    public SerialNumber CurrentValueMinusOne()
    {
        return new SerialNumber(this.Value - 1);
    }

    public SerialNumber CurrentValuePlusOne()
    {
        return new SerialNumber(this.Value + 1);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}