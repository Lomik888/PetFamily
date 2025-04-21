namespace PetFamily.Application.DTO.VolunteerDtos;

public record NameUpdateDto(
    string? FirstName,
    string? LastName,
    string? Surname
);