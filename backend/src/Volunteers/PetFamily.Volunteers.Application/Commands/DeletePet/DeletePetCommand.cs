using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Volunteers.Application.Commands.Delete;

namespace PetFamily.Volunteers.Application.Commands.DeletePet;

public record DeletePetCommand(
    Guid VolunteerId,
    Guid PetId,
    DeleteType DeleteType
) : ICommand;