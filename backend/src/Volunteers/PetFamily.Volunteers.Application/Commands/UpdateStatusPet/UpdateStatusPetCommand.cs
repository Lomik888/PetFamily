using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;

namespace PetFamily.Volunteers.Application.Commands.UpdateStatusPet;

public record UpdateStatusPetCommand(Guid VolunteerId, Guid PetId, HelpStatuses HelpStatus) : ICommand;