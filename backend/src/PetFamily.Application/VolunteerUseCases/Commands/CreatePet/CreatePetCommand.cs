using PetFamily.Application.Contracts.DTO.PetDtos;
using PetFamily.Application.Contracts.DTO.SharedDtos;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;

namespace PetFamily.Application.VolunteerUseCases.Commands.CreatePet;

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