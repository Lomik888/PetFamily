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
    public Volunteer VolunteerForEqual { get; private set; }
    public Volunteer Volunteer { get; private set; }
    public Pet SomePetForEqual { get; private set; }
    public Pet SomePet { get; private set; }

    private VolunteerFixture(
        RequestVolunteer requestVolunteer,
        RequestPet requestSomePet,
        List<RequestPet> requestPets)
    {
        Volunteer = CreateVolunteer(requestVolunteer);
        VolunteerForEqual = CreateVolunteer(requestVolunteer);

        SomePet = CreatePet(requestSomePet);
        SomePetForEqual = CreatePet(requestSomePet);

        switch (requestPets.Count)
        {
            case > COUNT_PET:
                throw new Exception("Too many request pets");
            case > 0:
            {
                var pets = new List<Pet>();
                var petsForEqual = new List<Pet>();
                for (int i = 0; i < COUNT_PET; i++)
                {
                    var pet = CreatePet(requestPets[i]);
                    var petForEqual = CreatePet(requestPets[i]);
                    pets.Add(pet);
                    petsForEqual.Add(petForEqual);
                }

                Volunteer.AddPets(pets);
                VolunteerForEqual.AddPets(petsForEqual);
                break;
            }
        }
    }

    public static VolunteerFixture Create(
        RequestVolunteer requestVolunteer,
        RequestPet requestSomePet,
        List<RequestPet> requestPets)
    {
        return new VolunteerFixture(
            requestVolunteer,
            requestSomePet,
            requestPets);
    }

    public static VolunteerFixture CreateWithOutPets(
        RequestVolunteer requestVolunteer,
        RequestPet requestSomePet)
    {
        return new VolunteerFixture(
            requestVolunteer,
            requestSomePet,
            []);
    }

    private static Pet CreatePet(RequestPet requestPet)
    {
        return new Pet(
            PetId.Create(requestPet.PetId).Value,
            NickName.Create(requestPet.Name).Value,
            SerialNumber.Create((uint)requestPet.SerialNumber).Value,
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
            CreatedAt.Create(requestPet.CreatedAt).Value,
            DetailsForHelps.CreateEmpty().Value,
            FilesPet.CreateEmpty().Value
        );
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