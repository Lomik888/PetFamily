using System.Collections;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.SharedKernel.Errors;

public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    public IReadOnlyList<Error> Errors => _errors;

    public int Count => _errors.Count;

    public Error this[int index] => _errors[index];

    private ErrorList(IEnumerable<Error> errors)
    {
        _errors = errors.ToList();
    }

    public static ErrorList Create(IEnumerable<Error> errors)
    {
        var errorsList = errors.ToList();

        Validator.Guard.NotNull(errorsList);

        return new ErrorList(errorsList);
    }

    public static ErrorList Create(Error error)
    {
        Validator.Guard.NotNull(error);

        return new ErrorList([error]);
    }

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}