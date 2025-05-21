using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.DTO.PetDtos;
using PetFamily.Application.Contracts.DTO.SharedDtos;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateFullPet;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;

namespace PetFamily.API.Contracts.Requests.Volunteer;

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
                : new FilesPetDto(FilesPetDto.FileDtos)
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