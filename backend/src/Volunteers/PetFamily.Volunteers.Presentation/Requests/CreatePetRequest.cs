using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.CreatePet;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record CreatePetRequest(
    Guid VolunteerId,
    string NickName,
    Guid SpeciesId,
    Guid BreedId,
    uint Age,
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
    IEnumerable<DetailsForHelpDto> DetailsForHelps
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
        var phoneNumberDto = new PhoneNumberDto(RegionCode, Number);

        return new CreatePetCommand(
            VolunteerId,
            NickName,
            speciesBreedIdDto,
            Age,
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