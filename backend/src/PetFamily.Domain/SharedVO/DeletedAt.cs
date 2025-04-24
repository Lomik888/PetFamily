using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.SharedVO;

public class DeletedAt : ValueObject
{
    public DateTime Value { get; }

    private DeletedAt(DateTime value)
    {
        Value = value;
    }

    public static Result<DeletedAt, Error> Create(DateTime value)
    {
        if (value > DateTime.UtcNow || value.Kind != DateTimeKind.Utc)
        {
            return ErrorsPreform.General.Validation("Invalid DateTime value", nameof(DeletedAt));
        }

        return new DeletedAt(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}