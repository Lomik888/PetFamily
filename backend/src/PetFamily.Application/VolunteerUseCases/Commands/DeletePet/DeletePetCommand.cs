using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Commands.Delete;

namespace PetFamily.Application.VolunteerUseCases.Commands.DeletePet;

public record DeletePetCommand(
    Guid VolunteerId,
    Guid PetId,
    DeleteType DeleteType
) : ICommand;