using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.Commands.UploadPetFiles;

public record UploadPetFilesCommand(
    IEnumerable<UploadFileDto> PetFilesDtos,
    Guid VolunteerId,
    Guid PetId) : ICommand;