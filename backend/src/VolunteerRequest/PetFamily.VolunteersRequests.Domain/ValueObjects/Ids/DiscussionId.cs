using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

public class DiscussionId : ValueObject
{
    public Guid Value { get; }

    protected DiscussionId(Guid value)
    {
        Value = value;
    }

    public static DiscussionId Create()
    {
        return new DiscussionId(Guid.NewGuid());
    }

    public static Result<DiscussionId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Pet id invalid", nameof(id));
        }

        return new DiscussionId(id);
    }

    public static DiscussionId CreateEmpty()
    {
        return new DiscussionId(Guid.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}