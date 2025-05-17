using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Commands.MovePet;

public record MovePetCommand(
    uint SerialNumber,
    Guid PetId,
    Guid VolunteerId
) : ICommand;