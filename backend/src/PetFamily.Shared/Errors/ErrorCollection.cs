using PetFamily.Shared.Errors.Interfaces;

namespace PetFamily.Shared.Errors;

public class ErrorCollection : IError
{
    private List<Error> _errors;

    public IReadOnlyList<Error> Errors => _errors;

    private ErrorCollection(IEnumerable<Error> errors)
    {
        _errors = errors.ToList();
    }

    public static ErrorCollection Create(IEnumerable<Error> errors) => new(errors);
}