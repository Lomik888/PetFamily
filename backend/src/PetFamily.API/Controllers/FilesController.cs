using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Contracts.Response.Envelope;
using PetFamily.API.Extensions;
using PetFamily.Application.Contracts.DTO.PetDtos;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Providers;
using PetFamily.Application.VolunteerUseCases.DeletePetFiles;
using PetFamily.Application.VolunteerUseCases.UploadPetFiles;
using PetFamily.Infrastructure.Providers.MinIo;
using PetFamily.Shared.Errors;

namespace PetFamily.API.Controllers;

public class FilesController : ApplicationController
{
    [HttpPost("{volunteerId:guid}/pet-files/{petId:guid}")]
    public async Task<IActionResult> UploadPetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] ICommandHandler<ErrorList, UploadPetFilesCommand> handler,
        CancellationToken cancellationToken)
    {
        await using var fileProcess = new UploadPetFileProcess();
        var filesDto = fileProcess.Process(files);
        var command = new UploadPetFilesCommand(filesDto, volunteerId, petId);

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
        [FromBody] IEnumerable<string> filesPaths,
        [FromServices] ICommandHandler<ErrorList, DeletePetFilesCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeletePetFilesCommand(filesPaths, volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }
}