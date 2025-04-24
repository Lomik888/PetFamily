using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.SoftDelete;

namespace PetFamily.Application.VolunteerUseCases.Delete;

public record DeleteVolunteerCommand(
    Guid VolunteerId,
    DeleteType DeleteType
) : ICommand;