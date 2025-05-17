using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.DeletePetFiles;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record DeletePetFilesRequest(IEnumerable<string> FilesPaths) : IToCommand<DeletePetFilesCommand, Guid, Guid>
{
    public DeletePetFilesCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new DeletePetFilesCommand(FilesPaths, volunteerId, petId);
    }
}