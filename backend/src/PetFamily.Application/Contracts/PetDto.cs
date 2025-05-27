using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Domain.VolunteerContext.SharedVO;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Application.Contracts;

public record PetDto(
    Guid VolunteerId,
    string FullName,
    string NickName,
    string SpecieName,
    string BreedName,
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
    double Height,
    double Weight,
    string FullPhoneNumber,
    bool Sterilize,
    DateTime DateOfBirth,
    bool Vaccinated,
    HelpStatuses HelpStatus,
    IEnumerable<DetailsForHelp> DetailsForHelps,
    IEnumerable<File> FilesPet
);