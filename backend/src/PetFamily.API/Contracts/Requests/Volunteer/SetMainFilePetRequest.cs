using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.SetMainFilePet;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record SetMainFilePetRequest(string FullPath) : IToCommand<SetMainFilePetCommand, Guid, Guid>
{
    public SetMainFilePetCommand ToCommand(Guid volunterId, Guid petId)
    {
        return new SetMainFilePetCommand(volunterId, petId, FullPath);
    }
}