namespace PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

public record NameUpdateDto(
    string? FirstName,
    string? LastName,
    string? Surname
);