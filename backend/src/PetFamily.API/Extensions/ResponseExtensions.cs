using System.Net;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Response.Envelope;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Errors.Enums;

namespace PetFamily.API.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToErrorActionResult(this Error error)
    {
        var status = GetStatusCode(error.ErrorType);
        var envelope = Envelope.Error(error.ToErrorResponse());

        return new ObjectResult(envelope)
        {
            StatusCode = (int)status,
        };
    }

    public static ActionResult ToErrorActionResult(this IEnumerable<Error> errors)
    {
        var array = errors.ToList();

        var errorResponses = array.ToErrorResponse();

        var envelope = Envelope.Error(errorResponses);

        if (array.Count != 0)
        {
            return new ObjectResult(envelope)
            {
                StatusCode = (int)StatusCodes.Status500InternalServerError,
            };
        }

        var status = GetStatusCode(array.First().ErrorType);

        return new ObjectResult(envelope)
        {
            StatusCode = (int)status,
        };
    }

    private static int GetStatusCode(ErrorType error)
    {
        var status = error switch
        {
            ErrorType.VALIDATION => HttpStatusCode.BadRequest,
            ErrorType.NOTFOUND => HttpStatusCode.NotFound,
            ErrorType.NONE => HttpStatusCode.BadRequest,
            ErrorType.EXCEPTION => HttpStatusCode.InternalServerError,
            ErrorType.UNKNOWNERROR => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError,
        };

        return (int)status;
    }
}