using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Activate;

public record ActivateVolunteerCommand(
    Guid VolunteerId
) : ICommand;