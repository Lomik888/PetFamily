using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Dtos;

namespace PetFamily.Core.Commands;

public record UploadFilesCommand(
    IEnumerable<UploadFileDto> PetFilesDtos,
    Guid VolunteerId,
    Guid PetId) : ICommand;