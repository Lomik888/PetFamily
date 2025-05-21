using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateStatusPet;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record UpdateStatusPetRequest(HelpStatuses HelpStatus) : IToCommand<UpdateStatusPetCommand, Guid, Guid>
{
    public UpdateStatusPetCommand ToCommand(Guid volunterId, Guid petId)
    {
        return new UpdateStatusPetCommand(volunterId, petId, HelpStatus);
    }
}