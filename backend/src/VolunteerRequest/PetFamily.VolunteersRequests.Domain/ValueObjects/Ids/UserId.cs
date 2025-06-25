using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

public class UserId : ValueObject
{
    public Guid Value { get; }

    protected UserId(Guid value)
    {
        Value = value;
    }

    public static UserId Create()
    {
        return new UserId(Guid.NewGuid());
    }

    public static Result<UserId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Pet id invalid", nameof(id));
        }

        return new UserId(id);
    }

    public static UserId CreateEmpty()
    {
        return new UserId(Guid.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}