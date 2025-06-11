using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.Delete;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record DeleteVolunteersRequest(
    DeleteType DeleteType
) : IToCommand<DeleteVolunteerCommand, Guid>
{
    public DeleteVolunteerCommand ToCommand(Guid volunteerId)
    {
        return new DeleteVolunteerCommand(volunteerId, DeleteType);
    }
}