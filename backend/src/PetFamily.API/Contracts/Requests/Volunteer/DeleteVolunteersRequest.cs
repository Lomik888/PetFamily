using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Delete;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record DeleteVolunteersRequest(
    DeleteType DeleteType
) : IToCommand<DeleteVolunteerCommand, Guid>
{
    public DeleteVolunteerCommand ToCommand(Guid volunteerId)
    {
        return new DeleteVolunteerCommand(volunteerId, DeleteType);
    }
}