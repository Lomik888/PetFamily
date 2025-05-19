using PetFamily.Application.Contracts.DTO.SharedDtos;

namespace PetFamily.Application.VolunteerUseCases.Commands.UpdateFullPet;

public record FilesPetDto(IReadOnlyList<FileDto> FileDtos);