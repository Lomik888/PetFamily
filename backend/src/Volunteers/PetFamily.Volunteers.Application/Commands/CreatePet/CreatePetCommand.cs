using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;

namespace PetFamily.Volunteers.Application.Commands.CreatePet;

public record CreatePetCommand(
    Guid VolunteerId,
    string NickName,
    SpeciesBreedIdDto SpeciesBreedIdDto,
    uint Age,
    string Description,
    string Color,
    HealthDescriptionDto HealthDescriptionDto,
    AddressDto AddressDto,
    double Weight,
    double Height,
    PhoneNumberDto PhoneNumberDto,
    bool Sterilize,
    DateTime DateOfBirth,
    bool Vaccinated,
    HelpStatuses HelpStatus,
    IEnumerable<DetailsForHelpDto> DetailsForHelps
) : ICommand;