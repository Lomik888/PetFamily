using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Discussions.Domain.ValueObjects.Ids;

public class MessageId : ComparableValueObject
{
    public Guid Value { get; }

    private MessageId(Guid value)
    {
        Value = value;
    }

    public static MessageId Create()
    {
        return new MessageId(Guid.NewGuid());
    }

    public static Result<MessageId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Id id invalid", nameof(MessageId));
        }

        return new MessageId(id);
    }

    public static MessageId CreateEmpty()
    {
        return new MessageId(Guid.Empty);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}