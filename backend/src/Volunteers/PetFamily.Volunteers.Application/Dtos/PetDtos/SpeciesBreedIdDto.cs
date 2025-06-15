namespace PetFamily.Volunteers.Application.Dtos.PetDtos;

public record SpeciesBreedIdDto(
    Guid SpeciesId,
    Guid BreedId);

public record SpeciesBreedIdNullableDto(
    Guid? SpeciesId,
    Guid? BreedId);