using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.MovePet;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record UpdateSerialNumberPetRequest(uint SerialNumber)
    : IToCommand<MovePetCommand, Guid, Guid>
{
    public MovePetCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new MovePetCommand(SerialNumber, volunteerId, petId);
    }
}