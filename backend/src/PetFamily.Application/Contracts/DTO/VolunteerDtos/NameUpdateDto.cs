namespace PetFamily.Application.Contracts.DTO.VolunteerDtos;

public record NameUpdateDto(
    string? FirstName,
    string? LastName,
    string? Surname
);