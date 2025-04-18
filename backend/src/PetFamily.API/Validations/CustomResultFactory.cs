using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetFamily.API.Extensions;
using PetFamily.API.Response.Envelope;
using PetFamily.Shared.Errors;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace PetFamily.API.Validations;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        ArgumentNullException.ThrowIfNull(validationProblemDetails);

        var errors = validationProblemDetails.Errors
            .SelectMany(x => x.Value.Select(y => Error.Deserialize(y).ToErrorResponse()))
            .ToList();

        var envelope = Envelope.Error(errors);

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest,
        };
    }
}