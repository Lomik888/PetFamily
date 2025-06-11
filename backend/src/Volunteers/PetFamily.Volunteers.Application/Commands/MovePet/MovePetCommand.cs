using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Commands.MovePet;

public record MovePetCommand(
    uint SerialNumber,
    Guid PetId,
    Guid VolunteerId
) : ICommand;