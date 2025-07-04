﻿namespace PetFamily.Volunteers.Application.Dtos.PetDtos;

public record HealthDescriptionDto(
    string SharedHealthStatus,
    string SkinCondition,
    string MouthCondition,
    string DigestiveSystemCondition);

public record HealthDescriptionNullableDto(
    string? SharedHealthStatus,
    string? SkinCondition,
    string? MouthCondition,
    string? DigestiveSystemCondition);