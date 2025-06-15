using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Specieses.Domain.Ids;

public class SpeciesId : ValueObject, IComparable<SpeciesId>
{
    public Guid Value { get; }

    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public static Result<SpeciesId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Species id is invalid", nameof(SpeciesId));
        }

        return new SpeciesId(id);
    }

    public static Result<SpeciesId> Create()
    {
        return new SpeciesId(Guid.NewGuid());
    }

    public static Result<SpeciesId> CreateEmpty()
    {
        return new SpeciesId(Guid.Empty);
    }

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