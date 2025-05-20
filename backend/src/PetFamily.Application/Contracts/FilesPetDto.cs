using PetFamily.Application.Contracts.DTO.SharedDtos;

namespace PetFamily.Application.Contracts;

public record FilesPetDto(IReadOnlyList<FileDto> FileDtos);