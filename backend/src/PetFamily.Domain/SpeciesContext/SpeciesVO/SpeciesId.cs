using CSharpFunctionalExtensions;

namespace PetFamily.Domain.SpeciesContext.SpeciesVO;

public class SpeciesId : ValueObject, IComparable<SpeciesId>
{
    public Guid Value { get; }

    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public static SpeciesId Create(Guid id) => new(id);
    public static SpeciesId Create() => new(Guid.NewGuid());
    public static SpeciesId CreateEmpty() => new(Guid.Empty);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public int CompareTo(SpeciesId? other)
    {
        if (object.ReferenceEquals(other, null))
        {
            return 1;
        }

        return (object)this == (object)other ? 0 : this.Value.CompareTo(other.Value);
    }
}