namespace PetFamily.Core.Dtos;

public record UploadFileDto(
    Stream FileStream,
    FileInfoDto FileInfoDto);