namespace PetFamily.Application.Contracts.DTO.PetDtos;

public record SpeciesBreedIdDto(
    Guid SpeciesId,
    Guid BreedId);

public record SpeciesBreedIdNullableDto(
    Guid? SpeciesId,
    Guid? BreedId);