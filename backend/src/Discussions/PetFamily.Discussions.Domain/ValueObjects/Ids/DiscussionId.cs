using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Discussions.Domain.ValueObjects.Ids;

public class DiscussionId : ComparableValueObject
{
    public Guid Value { get; }

    private DiscussionId(Guid value)
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
            return ErrorsPreform.General.Validation("Id id invalid", nameof(MessageId));
        }

        return new DiscussionId(id);
    }

    public static DiscussionId CreateEmpty()
    {
        return new DiscussionId(Guid.Empty);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}