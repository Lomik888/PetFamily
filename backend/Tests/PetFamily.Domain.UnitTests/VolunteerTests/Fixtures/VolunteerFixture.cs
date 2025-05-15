using PetFamily.Domain.Contracts;
using PetFamily.Domain.UnitTests.VolunteerTests.Builders;
using PetFamily.Domain.UnitTests.VolunteerTests.Requests;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

namespace PetFamily.Domain.UnitTests.VolunteerTests.Fixtures;

public class VolunteerFixture
{
    public const int COUNT_PET = 10;
    public Volunteer VolunteerForEqual => (Volunteer)Volunteer.Clone();
    public Volunteer Volunteer { get; private set; }
    public Volunteer SomeVolunteerWithOnePetForEqual => (Volunteer)SomeVolunteerWithOnePet.Clone();
    public Volunteer SomeVolunteerWithOnePet { get; private set; }
    public Pet SomePetForEqual => SomeVolunteerWithOnePetForEqual.Pets.First();
    public Pet SomePet => SomeVolunteerWithOnePet.Pets.First();

    private VolunteerFixture(
        RequestVolunteer requestVolunteer,
        RequestVolunteer requestSomeVolunteer,
        RequestPet requestSomePet,
        List<RequestPet> requestPets)
    {
        Volunteer = CreateVolunteer(requestVolunteer);
        SomeVolunteerWithOnePet = CreateVolunteer(requestSomeVolunteer);

        CreatePet(SomeVolunteerWithOnePet, requestSomePet);

        switch (requestPets.Count)
        {
            case > COUNT_PET:
                throw new Exception("Too many request pets");
            case > 0:
            {
                for (int i = 0; i < COUNT_PET; i++)
                {
                    CreatePet(Volunteer, requestPets[i]);
                }

                break;
            }
        }
    }

    public static VolunteerFixture Create(
        RequestVolunteer requestVolunteer,
        RequestVolunteer requestSomeVolunteer,
        RequestPet requestSomePet,
        List<RequestPet> requestPets)
    {
        return new VolunteerFixture(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet,
            requestPets);
    }

    public static VolunteerFixture CreateWithOutPets(
        RequestVolunteer requestVolunteer,
        RequestVolunteer requestSomeVolunteer,
        RequestPet requestSomePet)
    {
        return new VolunteerFixture(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet,
            []);
    }

    private static void CreatePet(Volunteer volunteer, RequestPet requestPet)
    {
        var createPetDto = new CreatePetDto(
            NickName.Create(requestPet.Name).Value,
            SpeciesBreedId.Create(requestPet.SpeciesId, requestPet.BreedId).Value,
            Description.Create(requestPet.Description).Value,
            Color.Create(requestPet.Color).Value,
            HealthDescription.Create(
                requestPet.SharedHealthStatus,
                requestPet.SkinCondition,
                requestPet.MouthCondition,
                requestPet.DigestiveSystemCondition).Value,
            Address.Create(
                requestPet.Country,
                requestPet.City,
                requestPet.Street,
                requestPet.HouseNumber,
                requestPet.ApartmentNumber).Value,
            Weight.Create(requestPet.Weight).Value,
            Height.Create(requestPet.Height).Value,
            PhoneNumber.Create(requestPet.RegionCode, requestPet.Number).Value,
            requestPet.Sterilize,
            DateOfBirth.Create(requestPet.DateOfBirth).Value,
            requestPet.Vaccinated,
            HelpStatus.Create(requestPet.HelpStatus).Value,
            DetailsForHelps.CreateEmpty().Value,
            FilesPet.CreateEmpty().Value);
        var petResult = volunteer.CreatePet(createPetDto);
        if (petResult.IsSuccess == false)
        {
            throw new Exception($"can't create pet {petResult.Error}");
        }
    }

    private static Volunteer CreateVolunteer(RequestVolunteer requestVolunteer)
    {
        return new Volunteer(
            VolunteerId.Create(requestVolunteer.VolunteerId).Value,
            Name.Create(
                requestVolunteer.FirstName,
                requestVolunteer.LastName,
                requestVolunteer.Surname).Value,
            Email.Create(requestVolunteer.Email).Value,
            Description.Create(requestVolunteer.Description).Value,
            Experience.Create(requestVolunteer.Experience).Value,
            PhoneNumber.Create(requestVolunteer.RegionCode, requestVolunteer.Number).Value,
            SocialNetworks.CreateEmpty().Value,
            DetailsForHelps.CreateEmpty().Value,
            Files.CreateEmpty().Value);
    }
}