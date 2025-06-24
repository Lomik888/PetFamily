using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel.Errors;

namespace PetFemily.Accounts.Application.Extensions;

public static class IdentityErrorExtensions
{
    public static IEnumerable<Error> ToErrors(this IEnumerable<IdentityError> errors)
    {
        return errors.Select(x => ErrorsPreform.General.IternalServerError(x.Description));
    }
}