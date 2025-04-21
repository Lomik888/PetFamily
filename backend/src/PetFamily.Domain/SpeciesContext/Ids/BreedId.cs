using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.SpeciesContext.Ids;

public class BreedId : ValueObject, IComparable<BreedId>
{
    public Guid Value { get; }

    private BreedId(Guid value)
    {
        Value = value;
    }

    public static Result<BreedId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Breed id is invalid", nameof(BreedId));
        }

        return new BreedId(id);
    }

    public static Result<BreedId, Error> Create()
    {
        return new BreedId(Guid.NewGuid());
    }

    public static Result<BreedId, Error> CreateEmpty()
    {
        return new BreedId(Guid.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public int CompareTo(BreedId? other)
    {
        if (object.ReferenceEquals(other, null))
        {
            return 1;
        }

        return (object)this == (object)other ? 0 : this.Value.CompareTo(other.Value);
    }
}