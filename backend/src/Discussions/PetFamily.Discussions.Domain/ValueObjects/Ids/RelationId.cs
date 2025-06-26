using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Discussions.Domain.ValueObjects.Ids;

public class RelationId : ValueObject
{
    public Guid Value { get; }

    private RelationId(Guid value)
    {
        Value = value;
    }

    public static RelationId Create()
    {
        return new RelationId(Guid.NewGuid());
    }

    public static Result<RelationId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Id is invalid", nameof(MessageId));
        }

        return new RelationId(id);
    }

    public static RelationId CreateEmpty()
    {
        return new RelationId(Guid.Empty);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}