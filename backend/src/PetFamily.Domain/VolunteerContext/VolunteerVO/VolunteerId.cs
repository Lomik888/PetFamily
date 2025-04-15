using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public class VolunteerId : ValueObject, IComparable<VolunteerId>
{
    public Guid Value { get; }

    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public static VolunteerId Create() => new(Guid.NewGuid());
    public static VolunteerId CreateEmpty() => new(Guid.Empty);

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