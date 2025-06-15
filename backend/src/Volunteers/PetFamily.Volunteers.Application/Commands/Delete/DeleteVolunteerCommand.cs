using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Commands.Delete;

public record DeleteVolunteerCommand(
    Guid VolunteerId,
    DeleteType DeleteType
) : ICommand;