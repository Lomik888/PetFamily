using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Commands.DeletePetFiles;

public record DeletePetFilesCommand(
    IEnumerable<string> FullFilePath,
    Guid VolunteerId,
    Guid PetId) : ICommand;