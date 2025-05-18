using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Commands.Activate;

public record ActivateVolunteerCommand(
    Guid VolunteerId
) : ICommand;