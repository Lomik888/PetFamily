using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Requests.Volunteer;
using PetFamily.API.Contracts.Response.Envelope;
using PetFamily.API.Extensions;
using PetFamily.Application.Contracts.DTO;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Commands.Activate;
using PetFamily.Application.VolunteerUseCases.Commands.Create;
using PetFamily.Application.VolunteerUseCases.Commands.CreatePet;
using PetFamily.Application.VolunteerUseCases.Commands.Delete;
using PetFamily.Application.VolunteerUseCases.Commands.MovePet;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateDetailsForHelps;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateMainInfo;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateSocialNetworks;
using PetFamily.Application.VolunteerUseCases.Queries;
using PetFamily.Application.VolunteerUseCases.Queries.Get;
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

    [HttpPut("{volunteerId:guid}/pets/{petId:guid}")]
    public async Task<ActionResult> UpdateSerialNumberPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<ErrorList, MovePetCommand> handler,
        [FromBody] UpdateSerialNumberPetRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HttpPost("{volunteerId:guid}/pets")]
    public async Task<ActionResult> UpdateSocials(
        [FromRoute] Guid volunteerId,
        [FromServices] ICommandHandler<ErrorList, CreatePetCommand> handler,
        [FromBody] CreatePetRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

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