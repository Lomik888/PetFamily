using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Collections;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO.Collections;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Domain.Dtos;

public record UpdatePetFullInfoDto(
    NickName NickName,
    SpeciesBreedId SpeciesBreedId,
    Description Description,
    Color Color,
    HealthDescription HealthDescription,
    Address Address,
    Weight Weight,
    Height Height,
    PhoneNumber PhoneNumber,
    bool Sterilize,
    DateOfBirth DateOfBirth,
    bool Vaccinated,
    HelpStatus HelpStatus,
    DetailsForHelps DetailsForHelps,
    FilesPet FilesPet);