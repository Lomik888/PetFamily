using PetFamily.Domain.VolunteerContext.PetsVO.Enums;

namespace PetFamily.Data.Tests.Requests;

public record RequestPet(
    string Name,
    Guid SpeciesId,
    Guid BreedId,
    uint Age,
    string Description,
    string Color,
    string SharedHealthStatus,
    string SkinCondition,
    string MouthCondition,
    string DigestiveSystemCondition,
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string ApartmentNumber,
    double Weight,
    double Height,
    string RegionCode,
    string Number,
    bool Sterilize,
    DateTime DateOfBirth,
    bool Vaccinated,
    HelpStatuses HelpStatus);