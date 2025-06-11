using PetFamily.Core.Dtos;
using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.UpdateFullPet;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record UpdateFullPetRequest(
    string? NickName,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Description,
    string? Color,
    string? SharedHealthStatus,
    string? SkinCondition,
    string? MouthCondition,
    string? DigestiveSystemCondition,
    AddressRequest? AddressDto,
    double? Weight,
    double? Height,
    PhoneNumberRequest? PhoneNumberDto,
    bool? Sterilize,
    DateTime? DateOfBirth,
    bool? Vaccinated,
    HelpStatuses? HelpStatus,
    DetailsForHelpCollectionRequest? DetailsForHelpCollectionDto,
    FilesPetRequest? FilesPetDto
) : IToCommand<UpdateFullPetCommand, Guid, Guid>
{
    public UpdateFullPetCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new UpdateFullPetCommand(
            volunteerId,
            petId,
            NickName,
            new SpeciesBreedIdNullableDto(SpeciesId, BreedId),
            Description,
            Color,
            new HealthDescriptionNullableDto(
                SharedHealthStatus,
                SkinCondition,
                MouthCondition,
                DigestiveSystemCondition),
            AddressDto == null
                ? null
                : new AddressDto(
                    AddressDto.Country,
                    AddressDto.City,
                    AddressDto.Street,
                    AddressDto.HouseNumber,
                    AddressDto.ApartmentNumber),
            Weight,
            Height,
            PhoneNumberDto == null
                ? null
                : new PhoneNumberDto(
                    PhoneNumberDto.RegionCode,
                    PhoneNumberDto.Number),
            Sterilize,
            DateOfBirth,
            Vaccinated,
            HelpStatus,
            DetailsForHelpCollectionDto == null
                ? null
                : new DetailsForHelpCollectionDto(DetailsForHelpCollectionDto.DetailsForHelps),
            FilesPetDto == null
                ? null
                : new FilesDto(FilesPetDto.FileDtos)
        );
    }
}

public record FilesPetRequest(IReadOnlyList<FileDto> FileDtos);

public record DetailsForHelpCollectionRequest(IReadOnlyList<DetailsForHelpDto> DetailsForHelps);

public record PhoneNumberRequest(
    string RegionCode,
    string Number);

public record AddressRequest(
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string ApartmentNumber);