using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.SetMainFilePet;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record SetMainFilePetRequest(string FullPath) : IToCommand<SetMainFilePetCommand, Guid, Guid>
{
    public SetMainFilePetCommand ToCommand(Guid volunterId, Guid petId)
    {
        return new SetMainFilePetCommand(volunterId, petId, FullPath);
    }
}