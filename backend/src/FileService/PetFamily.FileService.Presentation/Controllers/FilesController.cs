namespace PetFamily.FileService.Presentation.Controllers;

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
        await using var fileProcess = new UploadFileProcess();
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
}