using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Commands.Activate;

public record ActivateVolunteerCommand(
    Guid VolunteerId
) : ICommand;