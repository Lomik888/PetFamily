namespace PetFamily.Application.Contracts.DTO.PetDtos;

public record UploadPetFileDto(
    Stream FileStream,
    FileInfoDto FileInfoDto);