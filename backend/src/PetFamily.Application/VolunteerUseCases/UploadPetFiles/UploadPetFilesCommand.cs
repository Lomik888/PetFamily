using PetFamily.Application.Contracts.DTO.PetDtos;
using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.UploadPetFiles;

public record UploadPetFilesCommand(
    IEnumerable<UploadPetFileDto> PetFilesDtos,
    Guid VolunteerId,
    Guid PetId) : ICommand;