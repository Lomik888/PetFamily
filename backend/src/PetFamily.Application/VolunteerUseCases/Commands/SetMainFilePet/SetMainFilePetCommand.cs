using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Commands.SetMainFilePet;

public record SetMainFilePetCommand(Guid VolunteerId, Guid PetId, string FullPath) : ICommand;