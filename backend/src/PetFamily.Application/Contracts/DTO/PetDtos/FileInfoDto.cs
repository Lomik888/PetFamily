namespace PetFamily.Application.Contracts.DTO.PetDtos;

public record FileInfoDto(
    string Name,
    string Extension,
    long Size);