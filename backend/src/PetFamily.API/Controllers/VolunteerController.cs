using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Requests.Volunteer;
using PetFamily.API.Contracts.Response.Envelope;
using PetFamily.API.Extensions;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Activate;
using PetFamily.Application.VolunteerUseCases.Create;
using PetFamily.Application.VolunteerUseCases.Delete;
using PetFamily.Application.VolunteerUseCases.UpdateDetailsForHelps;
using PetFamily.Application.VolunteerUseCases.UpdateMainInfo;
using PetFamily.Application.VolunteerUseCases.UpdateSocialNetworks;
using PetFamily.Shared.Errors;

namespace PetFamily.API.Controllers;

public class VolunteerController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, ErrorList, CreateVolunteerCommand> handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPatch("{volunteerId:guid}/main-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid volunteerId,
        [FromServices] ICommandHandler<Guid, ErrorList, UpdateMainInfoVolunteerCommand> handler,
        [FromBody] UpdateMainInfoVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPut("{volunteerId:guid}/socials")]
    public async Task<ActionResult> Update(
        [FromRoute] Guid volunteerId,
        [FromServices] ICommandHandler<ErrorList, UpdateVolunteersSocialNetworksCommand> handler,
        [FromBody] UpdateVolunteersSocialNetworksRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HttpPut("{volunteerId:guid}/details-for-help")]
    public async Task<ActionResult> Update(
        [FromRoute] Guid volunteerId,
        [FromServices] ICommandHandler<ErrorList, UpdateVolunteersDetailsForHelpCommand> handler,
        [FromBody] UpdateVolunteersDetailsForHelpRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HttpPut("{volunteerId:guid}/account-status")]
    public async Task<ActionResult> DeleteAccount(
        [FromRoute] Guid volunteerId,
        [FromServices] ICommandHandler<ErrorList, DeleteVolunteerCommand> handler,
        [FromBody] DeleteVolunteersRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HttpPut("{volunteerId:guid}/")]
    public async Task<ActionResult> ActivateAccount(
        [FromRoute] Guid volunteerId,
        [FromServices] ICommandHandler<ErrorList, ActivateVolunteerCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(new ActivateVolunteerCommand(volunteerId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }
}