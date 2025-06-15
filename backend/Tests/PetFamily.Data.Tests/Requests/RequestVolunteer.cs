namespace PetFamily.Data.Tests.Requests;

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