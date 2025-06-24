using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.SharedKernel.ValueObjects;

public class BaseVoId : ValueObject
{
    public Guid Value { get; }

    protected BaseVoId(Guid value)
    {
        Value = value;
    }

    public static BaseVoId Create()
    {
        return new BaseVoId(Guid.NewGuid());
    }

    public static Result<BaseVoId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Pet id invalid", nameof(id));
        }

        return new BaseVoId(id);
    }

    public static BaseVoId CreateEmpty()
    {
        return new BaseVoId(Guid.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}