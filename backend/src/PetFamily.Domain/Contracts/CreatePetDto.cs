using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;

namespace PetFamily.Domain.Contracts;

public record CreatePetDto(
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