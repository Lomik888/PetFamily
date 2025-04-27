using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.IdsVO;

public class VolunteerId : ValueObject, IComparable<VolunteerId>
{
    public Guid Value { get; }

    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public static Result<VolunteerId> Create()
    {
        return new VolunteerId(Guid.NewGuid());
    }

    public static Result<VolunteerId> CreateEmpty()
    {
        return new VolunteerId(Guid.Empty);
    }

    public static Result<VolunteerId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Volunteer id is invalid", nameof(VolunteerId));
        }

        return new VolunteerId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public int CompareTo(VolunteerId? other)
    {
        if (object.ReferenceEquals(other, null))
        {
            return 1;
        }

        return (object)this == (object)other ? 0 : this.Value.CompareTo(other.Value);
    }
}