using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Framework;
using PetFamily.Framework.Abstractions;
using PetFamily.Framework.Extensions;
using PetFamily.Framework.Responses;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.Activate;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Application.Commands.CreatePet;
using PetFamily.Volunteers.Application.Commands.Delete;
using PetFamily.Volunteers.Application.Commands.DeletePet;
using PetFamily.Volunteers.Application.Commands.MovePet;
using PetFamily.Volunteers.Application.Commands.SetMainFilePet;
using PetFamily.Volunteers.Application.Commands.UpdateDetailsForHelps;
using PetFamily.Volunteers.Application.Commands.UpdateFullPet;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Application.Commands.UpdateSocialNetworks;
using PetFamily.Volunteers.Application.Commands.UpdateStatusPet;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;
using PetFamily.Volunteers.Application.Queries.Get;
using PetFamily.Volunteers.Presentation.Requests;

namespace PetFamily.Volunteers.Presentation.Controllers;

public class VolunteerController : ApplicationController
{
    [HasPermission(PermissionTypes.VolunteersModule.Volunteer.Create)]
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

    [HasPermission(PermissionTypes.VolunteersModule.Volunteer.UpdateMainInfo)]
    [HttpPatch("{volunteerId:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
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

    [HasPermission(PermissionTypes.VolunteersModule.Volunteer.UpdateSocials)]
    [HttpPut("{volunteerId:guid}/socials")]
    public async Task<ActionResult> UpdateSocials(
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

    [HasPermission(PermissionTypes.VolunteersModule.Volunteer.UpdateDetailsForHelp)]
    [HttpPut("{volunteerId:guid}/details-for-help")]
    public async Task<ActionResult> UpdateDetailsForHelp(
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

    [HasPermission(PermissionTypes.VolunteersModule.Volunteer.DeleteAccount)]
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

    [AllowAnonymous]
    [HttpGet("volunteers")]
    public async Task<ActionResult> Get(
        [FromQuery] GetWithPagination request,
        [FromServices] IQueryHandler<GetObjectsWithPaginationResponse<VolunteerDto>, ErrorList, GetQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }

    [HasPermission(PermissionTypes.VolunteersModule.Volunteer.ActivateAccount)]
    [HttpPut("{volunteerId:guid}")]
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