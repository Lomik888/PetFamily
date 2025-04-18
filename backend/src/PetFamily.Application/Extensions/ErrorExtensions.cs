using FluentValidation.Results;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.Extensions;

public static class ErrorExtensions
{
    public static IEnumerable<Error> ToErrors(this List<ValidationFailure> errors)
    {
        return errors.Select(x => Error.Deserialize(x.ErrorMessage));
    }

    public static Error ToError(this List<ValidationFailure> errors)
    {
        if (errors.Count > 1)
        {
            throw new Exception("Method ToError returns no more than one error");
        }

        return errors.Select(x => Error.Deserialize(x.ErrorMessage)).First();
    }
}