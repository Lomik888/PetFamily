namespace PetFamily.Core.Dtos;

public record FileInfoDto(
    string Name,
    string Extension,
    long Size);