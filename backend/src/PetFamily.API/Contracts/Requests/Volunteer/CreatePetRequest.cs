using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.Contracts.DTO.PetDtos;
using PetFamily.Application.Contracts.DTO.SharedDtos;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.CreatePet;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PhoneNumberDto = PetFamily.Application.Contracts.DTO.VolunteerDtos.PhoneNumberDto;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record CreatePetRequest(
    Guid VolunteerId,
    string NickName,
    Guid SpeciesId,
    Guid BreedId,
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
    double Weight,
    double Height,
    string RegionCode,
    string Number,
    bool Sterilize,
    DateTime DateOfBirth,
    bool Vaccinated,
    HelpStatuses HelpStatus,
    List<DetailsForHelpDto> DetailsForHelps
) : IToCommand<CreatePetCommand>
{
    public CreatePetCommand ToCommand()
    {
        var speciesBreedIdDto = new SpeciesBreedIdDto(SpeciesId, BreedId);
        var healthDescriptionDto = new HealthDescriptionDto(
            SharedHealthStatus,
            SkinCondition,
            MouthCondition,
            DigestiveSystemCondition
        );
        var addressDto = new AddressDto(
            Country,
            City,
            Street,
            HouseNumber,
            ApartmentNumber
        );
        var phoneNumberDto = new Application.Contracts.DTO.PetDtos.PhoneNumberDto(RegionCode, Number);

        return new CreatePetCommand(
            VolunteerId,
            NickName,
            speciesBreedIdDto,
            Description,
            Color,
            healthDescriptionDto,
            addressDto,
            Weight,
            Height,
            phoneNumberDto,
            Sterilize,
            DateOfBirth,
            Vaccinated,
            HelpStatus,
            DetailsForHelps
        );
    }
}