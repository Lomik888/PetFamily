using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

public class AdminId : ValueObject
{
    public Guid Value { get; }

    protected AdminId(Guid value)
    {
        Value = value;
    }

    public static AdminId Create()
    {
        return new AdminId(Guid.NewGuid());
    }

    public static Result<AdminId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Id id invalid", nameof(id));
        }

        return new AdminId(id);
    }

    public static AdminId CreateEmpty()
    {
        return new AdminId(Guid.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}