using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.DeletePetFiles;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record DeletePetFilesRequest(IEnumerable<string> FilesPaths) : IToCommand<DeletePetFilesCommand, Guid, Guid>
{
    public DeletePetFilesCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new DeletePetFilesCommand(FilesPaths, volunteerId, petId);
    }
}