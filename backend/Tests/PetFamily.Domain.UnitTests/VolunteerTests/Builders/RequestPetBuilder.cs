using AutoFixture;
using PetFamily.Domain.UnitTests.AutoFixture;
using PetFamily.Domain.UnitTests.VolunteerTests.Fixtures;
using PetFamily.Domain.UnitTests.VolunteerTests.Requests;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;

namespace PetFamily.Domain.UnitTests.VolunteerTests.Builders;

public static class RequestPetBuilder
{
    private static readonly Fixture _autoFixture = AutoFixtureBuilder.FixtureBuild();
    private static uint _petSerialNumber = 1;

    public static RequestPet PetBuild()
    {
        return _autoFixture
            .Build<RequestPet>()
            .With(x => x.SpeciesId, Guid.NewGuid())
            .With(x => x.BreedId, Guid.NewGuid())
            .With(x =>
                    x.DateOfBirth,
                new DateTime(2025, 1, 2, 1, 1, 1, DateTimeKind.Utc))
            .With(x => x.HelpStatus, HelpStatuses.DONE)
            .With(x => x.HouseNumber, "4")
            .With(x => x.ApartmentNumber, "4")
            .With(x => x.Height, 10D)
            .With(x => x.Weight, 10D)
            .With(x => x.RegionCode, "+7")
            .With(x => x.Number, "9535793109")
            .Create();
    }

    public static RequestPet PetBuild(Guid speciesId, Guid breedId)
    {
        return _autoFixture
            .Build<RequestPet>()
            .With(x => x.SpeciesId, speciesId)
            .With(x => x.BreedId, breedId)
            .With(x =>
                    x.DateOfBirth,
                new DateTime(2025, 1, 2, 1, 1, 1, DateTimeKind.Utc))
            .With(x => x.HelpStatus, HelpStatuses.DONE)
            .With(x => x.HouseNumber, "4")
            .With(x => x.ApartmentNumber, "4")
            .With(x => x.Height, 10D)
            .With(x => x.Weight, 10D)
            .With(x => x.RegionCode, "+7")
            .With(x => x.Number, "9535793109")
            .Create();
    }

    public static IEnumerable<RequestPet> PetsBuild(int countPets = VolunteerFixture.COUNT_PET)
    {
        var requestPets = new List<RequestPet>();

        for (var i = 1; i <= countPets; i++)
        {
            _petSerialNumber = (uint)i;
            requestPets.Add(PetBuild());
        }

        return requestPets;
    }
}