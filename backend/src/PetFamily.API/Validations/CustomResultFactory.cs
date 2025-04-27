using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetFamily.API.Contracts.Response.Envelope;
using PetFamily.API.Extensions;
using PetFamily.Shared.Errors;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace PetFamily.API.Validations;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    private readonly ILogger<CustomResultFactory> _logger;

    public CustomResultFactory(ILogger<CustomResultFactory> logger)
    {
        _logger = logger;
    }

    public IActionResult CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        ArgumentNullException.ThrowIfNull(validationProblemDetails);

        _logger.LogInformation("Invalid Validation Request");
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