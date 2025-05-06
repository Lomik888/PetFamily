namespace PetFamily.Domain.UnitTests.VolunteerTests.Requests;

public record RequestVolunteer(
    Guid VolunteerId,
    string FirstName,
    string LastName,
    string Surname,
    string Email,
    string Description,
    int Experience,
    string RegionCode,
    string Number);