using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Delete;

public record DeleteVolunteerCommand(
    Guid VolunteerId,
    DeleteType DeleteType
) : ICommand;