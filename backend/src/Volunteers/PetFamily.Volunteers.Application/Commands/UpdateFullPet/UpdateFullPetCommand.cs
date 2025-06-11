using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Dtos;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;

namespace PetFamily.Volunteers.Application.Commands.UpdateFullPet;

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
    FilesDto? FilesPetDto) : ICommand;