using AutoFixture;
using PetFamily.Domain.UnitTests.AutoFixture;
using PetFamily.Domain.UnitTests.VolunteerTests.Requests;

namespace PetFamily.Domain.UnitTests.VolunteerTests.Builders;

public static class RequestSpeciesBuilder
{
    private static readonly Fixture _autoFixture = AutoFixtureBuilder.FixtureBuild();

    public static RequestSpecies SpeciesBuild()
    {
        return _autoFixture
            .Build<RequestSpecies>()
            .Create();
    }

    public static IEnumerable<RequestSpecies> SpeciesBuild(int countSpecies)
    {
        var requestSpecies = new List<RequestSpecies>();

        for (var i = 1; i <= countSpecies; i++)
        {
            requestSpecies.Add(SpeciesBuild());
        }

        return requestSpecies;
    }
}