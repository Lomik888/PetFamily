using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Commands.DeletePetFiles;

public record DeletePetFilesCommand(
    IEnumerable<string> FullFilePath,
    Guid VolunteerId,
    Guid PetId) : ICommand;