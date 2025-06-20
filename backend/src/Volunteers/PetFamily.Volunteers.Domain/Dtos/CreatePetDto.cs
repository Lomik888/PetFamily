using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Collections;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Domain.Dtos;

public record CreatePetDto(
    NickName NickName,
    SpeciesBreedId SpeciesBreedId,
    Age Age,
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