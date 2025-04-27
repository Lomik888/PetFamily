using PetFamily.Shared.Errors.Interfaces;

namespace PetFamily.Shared.Errors;

public record ErrorCollection : IError
{
    public readonly IReadOnlyList<Error> Errors;

    private ErrorCollection(IEnumerable<Error> errors)
    {
        Errors = errors.ToList();
    }

    public static ErrorCollection Create(IEnumerable<Error> errors) => new(errors);
}