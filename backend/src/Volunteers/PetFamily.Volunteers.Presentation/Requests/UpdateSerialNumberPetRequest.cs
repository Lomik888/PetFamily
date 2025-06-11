using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.MovePet;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record UpdateSerialNumberPetRequest(uint SerialNumber)
    : IToCommand<MovePetCommand, Guid, Guid>
{
    public MovePetCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new MovePetCommand(SerialNumber, volunteerId, petId);
    }
}