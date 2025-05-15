using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.MovePet;

public record MovePetCommand(
    uint SerialNumber,
    Guid PetId,
    Guid VolunteerId
) : ICommand;