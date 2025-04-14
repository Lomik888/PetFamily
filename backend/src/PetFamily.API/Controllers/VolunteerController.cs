using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Requests.Volunteer;
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
        
        return Ok(result.Value);
    }
}