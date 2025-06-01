using PetFamily.Data.Tests.Requests;
using PetFamily.Domain.Contracts;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;

namespace PetFamily.Data.Tests.Factories;

public static class PetsFactory
{
    public static Volunteer CreatePets(Volunteer volunteer, IEnumerable<RequestPet> requestPets)
    {
        foreach (var pet in requestPets)
        {
            CreatePet(volunteer, pet);
        }

        return volunteer;
    }

    public static Volunteer CreatePet(Volunteer volunteer, RequestPet requestPet)
    {
        var nickName = NickName.Create(requestPet.Name).Value;
        var speciesBreedId = SpeciesBreedId.Create(requestPet.SpeciesId, requestPet.BreedId).Value;
        var age = Age.Create(requestPet.Age).Value;
        var description = Description.Create(requestPet.Description).Value;
        var color = Color.Create(requestPet.Color).Value;
        var healthDescription = HealthDescription.Create(
            requestPet.SharedHealthStatus,
            requestPet.SkinCondition,
            requestPet.MouthCondition,
            requestPet.DigestiveSystemCondition).Value;
        var address = Address.Create(
            requestPet.Country,
            requestPet.City,
            requestPet.Street,
            requestPet.HouseNumber,
            requestPet.ApartmentNumber).Value;
        var weight = Weight.Create(requestPet.Weight).Value;
        var height = Height.Create(requestPet.Height).Value;
        var phoneNumber = PhoneNumber.Create(
            requestPet.RegionCode,
            requestPet.Number).Value;
        var dateOfBirth = DateOfBirth.Create(requestPet.DateOfBirth).Value;
        var helpStatus = HelpStatus.Create(requestPet.HelpStatus).Value;
        var detailsForHelps = DetailsForHelps.CreateEmpty().Value;
        var files = FilesPet.CreateEmpty().Value;
        var createPetDto = new CreatePetDto(
            nickName,
            speciesBreedId,
            age,
            description,
            color,
            healthDescription,
            address,
            weight,
            height,
            phoneNumber,
            requestPet.Sterilize,
            dateOfBirth,
            requestPet.Vaccinated,
            helpStatus,
            detailsForHelps,
            files);

        var petResult = volunteer.CreatePet(createPetDto);
        if (petResult.IsSuccess == false)
        {
            throw new Exception($"can't create pet {petResult.Error}");
        }

        return volunteer;
    }
}