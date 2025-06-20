using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Commands;
using PetFamily.Framework;
using PetFamily.Framework.Abstractions;
using PetFamily.Framework.Extensions;
using PetFamily.Framework.Responses;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.CreatePet;
using PetFamily.Volunteers.Application.Commands.DeletePet;
using PetFamily.Volunteers.Application.Commands.DeletePetFiles;
using PetFamily.Volunteers.Application.Commands.MovePet;
using PetFamily.Volunteers.Application.Commands.SetMainFilePet;
using PetFamily.Volunteers.Application.Commands.UpdateFullPet;
using PetFamily.Volunteers.Application.Commands.UpdateStatusPet;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Queries.GetPet;
using PetFamily.Volunteers.Application.Queries.GetPets;
using PetFamily.Volunteers.Presentation.Requests;

namespace PetFamily.Volunteers.Presentation.Controllers;

public class PetController : ApplicationController
{
    [HasPermission(PermissionTypes.VolunteersModule.Pet.UpdateMainInfo)]
    [HttpPatch("{volunteerId:guid}/{petId:guid}/full-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<ErrorList, UpdateFullPetCommand> handler,
        [FromBody] UpdateFullPetRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HasPermission(PermissionTypes.VolunteersModule.Pet.UpdateStatus)]
    [HttpPut("{volunteerId:guid}/{petId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdateStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<ErrorList, UpdateStatusPetCommand> handler,
        [FromBody] UpdateStatusPetRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HasPermission(PermissionTypes.VolunteersModule.Pet.DeletePet)]
    [HttpDelete("{volunteerId:guid}/{petId:guid}")]
    public async Task<ActionResult<Guid>> DeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<ErrorList, DeletePetCommand> handler,
        [FromBody] DeletePetRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HasPermission(PermissionTypes.VolunteersModule.Pet.UpdateSocials)]
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

    [HasPermission(PermissionTypes.VolunteersModule.Pet.UpdateSerialNumberPet)]
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

    [HasPermission(PermissionTypes.VolunteersModule.Pet.SetMainFilePet)]
    [HttpPut("{volunteerId:guid}/{petId:guid}/main-file")]
    public async Task<ActionResult<Guid>> SetMainFilePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] ICommandHandler<ErrorList, SetMainFilePetCommand> handler,
        [FromBody] SetMainFilePetRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [AllowAnonymous]
    [HttpGet("pet/{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid petId,
        [FromServices] IQueryHandler<PetDto, ErrorList, GetPetQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var request = new GetPetRequest();

        var result = await handler.Handle(request.ToQuery(petId), cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }

    [HasPermission(PermissionTypes.VolunteersModule.Pet.UploadPetFiles)]
    [HttpPost("{volunteerId:guid}/pet-files/{petId:guid}")]
    public async Task<IActionResult> UploadPetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] ICommandHandler<ErrorList, UploadFilesCommand> handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcess = new UploadFileProcess();
        var filesDto = fileProcess.Process(files);
        var command = new UploadFilesCommand(filesDto, volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [HasPermission(PermissionTypes.VolunteersModule.Pet.DeletePetFiles)]
    [HttpDelete("{volunteerId:guid}/pet-files/{petId:guid}")]
    public async Task<IActionResult> DeletePetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetFilesRequest request,
        [FromServices] ICommandHandler<ErrorList, DeletePetFilesCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(volunteerId, petId), cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [AllowAnonymous]
    [HttpGet("pets")]
    public async Task<ActionResult> GetPets(
        [FromBody] GetPetsRequest request,
        [FromServices] IQueryHandler<GetObjectsWithPaginationResponse<PetDto>, ErrorList, GetPetsQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }
}