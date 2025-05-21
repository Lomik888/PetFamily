using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.DTO.PetDtos;
using PetFamily.Application.Contracts.DTO.SharedDtos;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;

namespace PetFamily.Application.VolunteerUseCases.Commands.UpdateFullPet;

public record UpdateFullPetCommand(
    Guid VolunteerId,
    Guid PetId,
    string? NickName,
    SpeciesBreedIdNullableDto? SpeciesBreedId,
    string? Description,
    string? Color,
    HealthDescriptionNullableDto? HealthDescriptionDto, 
    AddressDto? AddressDto, 
    double? Weight,
    double? Height,
    PhoneNumberDto? PhoneNumberDto, 
    bool? Sterilize,
    DateTime? DateOfBirth,
    bool? Vaccinated,
    HelpStatuses? HelpStatus,
    DetailsForHelpCollectionDto? DetailsForHelps, 
    FilesPetDto? FilesPetDto) : ICommand; 