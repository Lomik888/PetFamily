namespace PetFamily.Application.Contracts.DTO.PetDtos;

public record HealthDescriptionDto(
    string SharedHealthStatus,
    string SkinCondition,
    string MouthCondition,
    string DigestiveSystemCondition);