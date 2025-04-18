using PetFamily.API.Response;
using PetFamily.Shared.Errors;

namespace PetFamily.API.Extensions;

public static class ErrorExtensions
{
    public static ErrorResponse ToErrorResponse(this Error error)
    {
        return new ErrorResponse(error.ErrorCode, error.Message, error.ErrorType.ToString());
    }

    public static IEnumerable<ErrorResponse> ToErrorResponse(this IEnumerable<Error> error)
    {
        return error.Select(x => new ErrorResponse(x.ErrorCode, x.Message, x.ErrorType.ToString()));
    }
}