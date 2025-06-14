using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Core;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Commands;
using PetFamily.Framework.Abstractions;
using PetFamily.Framework.Extensions;
using PetFamily.Framework.Responses;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.DeletePetFiles;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Queries.GetPet;
using PetFamily.Volunteers.Application.Queries.GetPets;
using PetFamily.Volunteers.Presentation.Requests;

namespace PetFamily.Volunteers.Presentation.Controllers;

public class PetController : ApplicationController
{
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