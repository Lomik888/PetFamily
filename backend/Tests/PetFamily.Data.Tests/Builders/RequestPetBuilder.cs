using AutoFixture;
using PetFamily.Data.Tests.Requests;
using PetFamily.Specieses.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

namespace PetFamily.Data.Tests.Builders;

public static class RequestPetBuilder
{
    private const int MAX_APARTMENT_NUMBE_RVALUE = Address.APARTMENTNUMBER_MAX_LENGHT * 10;
    private const int MAX_HOUSENUMBER_VALUE = Address.HOUSENUMBER_MAX_LENGHT * 10;
    private const double MAX_HEIGH_TVALUE = Height.VELUE_MAX_LENGHT;
    private const double MAX_WEIGHT_VALUE = Weight.VELUE_MAX_LENGHT;

    private static readonly Fixture _autoFixture = new Fixture();
    private static readonly Random _random = new Random();

    public static RequestPet PetBuild(Guid speciesId, Guid breedId)
    {
        var maxHeight = Convert.ToInt32(MAX_HEIGH_TVALUE);
        var maxWeight = Convert.ToInt32(MAX_WEIGHT_VALUE);
        var houseNumber = _random.Next(1, MAX_HOUSENUMBER_VALUE);
        var apartmentNumber = _random.Next(1, MAX_APARTMENT_NUMBE_RVALUE);
        var height = Convert.ToDouble(_random.Next(1, maxHeight));
        var weight = Convert.ToDouble(_random.Next(1, maxWeight));
        var regionCode = _random.Next(1, 10);
        var number = _random.Next(10, int.MaxValue);
        var age = _random.Next(0, (int)Age.VELUE_MAX + 1);
        var numberForName = _random.Next(0, 101);

        return _autoFixture
            .Build<RequestPet>()
            .With(x => x.DateOfBirth, DateTime.UtcNow)
            .With(x => x.SpeciesId, speciesId)
            .With(x => x.BreedId, breedId)
            .With(x => x.HouseNumber, houseNumber.ToString())
            .With(x => x.ApartmentNumber, apartmentNumber.ToString())
            .With(x => x.Name, $"Любовь{numberForName}")
            .With(x => x.Age, (uint)age)
            .With(x => x.Height, height)
            .With(x => x.Weight, weight)
            .With(x => x.RegionCode, $"+{regionCode}")
            .With(x => x.Number, number.ToString())
            .Create();
    }

    public static RequestPet PetBuild()
    {
        var maxHeight = Convert.ToInt32(MAX_HEIGH_TVALUE);
        var maxWeight = Convert.ToInt32(MAX_WEIGHT_VALUE);
        var houseNumber = _random.Next(1, MAX_HOUSENUMBER_VALUE);
        var apartmentNumber = _random.Next(1, MAX_APARTMENT_NUMBE_RVALUE);
        var height = Convert.ToDouble(_random.Next(1, maxHeight));
        var weight = Convert.ToDouble(_random.Next(1, maxWeight));
        var regionCode = _random.Next(1, 10);
        var number = _random.Next(10, int.MaxValue);
        var age = _random.Next(0, (int)Age.VELUE_MAX + 1);
        var numberForName = _random.Next(0, 101);

        return _autoFixture
            .Build<RequestPet>()
            .With(x => x.DateOfBirth, DateTime.UtcNow)
            .With(x => x.HouseNumber, houseNumber.ToString())
            .With(x => x.ApartmentNumber, apartmentNumber.ToString())
            .With(x => x.Height, height)
            .With(x => x.Name, $"Любовь{numberForName}")
            .With(x => x.Age, (uint)age)
            .With(x => x.Weight, weight)
            .With(x => x.RegionCode, $"+{regionCode}")
            .With(x => x.Number, number.ToString())
            .Create();
    }

    public static IEnumerable<RequestPet> PetsBuild(int countPets)
    {
        var requestPets = new List<RequestPet>();

        for (var i = 0; i < countPets; i++)
        {
            requestPets.Add(PetBuild());
        }

        return requestPets;
    }

    public static IEnumerable<RequestPet> PetsBuild(List<Species> species, int countPets)
    {
        var requestPets = new List<RequestPet>();

        for (var i = 0; i < countPets; i++)
        {
            var specieIndex = _random.Next(0, species.Count);
            var specie = species[specieIndex];
            var breedIndex = _random.Next(0, specie.Breeds.Count);
            var breed = specie.Breeds[breedIndex];
            var requestPet = PetBuild(specie.Id.Value, breed.Id.Value);

            requestPets.Add(requestPet);
        }

        return requestPets;
    }
}