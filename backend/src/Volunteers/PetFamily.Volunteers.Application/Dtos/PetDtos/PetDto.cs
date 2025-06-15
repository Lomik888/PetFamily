using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using File = PetFamily.Volunteers.Domain.ValueObjects.SharedVO.File;

namespace PetFamily.Volunteers.Application.Dtos.PetDtos;

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