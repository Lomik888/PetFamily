using PetFamily.API.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Delete;
using PetFamily.Application.VolunteerUseCases.SoftDelete;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record DeleteVolunteersRequest(
    DeleteType DeleteType
) : IToCommand<SoftDeleteVolunteerCommand, Guid>
{
    public SoftDeleteVolunteerCommand ToCommand(Guid volunteerId)
    {
        return new SoftDeleteVolunteerCommand(volunteerId, DeleteType);
    }
}