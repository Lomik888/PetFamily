using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests.Volunteer;
using PetFamily.API.Response.Envelope;
using PetFamily.Application.VolunteerUseCases.CreateVolunteer;

namespace PetFamily.API.Controllers;

public class VolunteerController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] ICreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await handler.Create(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }
}