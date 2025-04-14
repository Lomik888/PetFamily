using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class PetId : ValueObject, IComparable<PetId>
{
    public Guid Value { get; }

    private PetId(Guid value)
    {
        Value = value;
    }

    public static PetId Create() => new(Guid.NewGuid());
    public static PetId Create(Guid id) => new(id);
    public static PetId CreateEmpty() => new(Guid.Empty);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public int CompareTo(PetId? other)
    {
        if (object.ReferenceEquals(other, null))
        {
            return 1;
        }

        return (object)this == (object)other ? 0 : this.Value.CompareTo(other.Value);
    }
}