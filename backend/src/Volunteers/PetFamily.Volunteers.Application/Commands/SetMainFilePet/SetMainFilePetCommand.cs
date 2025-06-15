using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Commands.SetMainFilePet;

public record SetMainFilePetCommand(Guid VolunteerId, Guid PetId, string FullPath) : ICommand;