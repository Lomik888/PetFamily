using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;

namespace PetFamily.Application.VolunteerUseCases.Commands.UpdateStatusPet;

public record UpdateStatusPetCommand(Guid VolunteerId, Guid PetId, HelpStatuses HelpStatus) : ICommand;