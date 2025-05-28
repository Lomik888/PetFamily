using AutoFixture;
using PetFamily.Domain.UnitTests.AutoFixture;
using PetFamily.Domain.UnitTests.VolunteerTests.Requests;

namespace PetFamily.Domain.UnitTests.VolunteerTests.Builders;

public static class RequestBreedBuilder
{
    private static readonly Fixture _autoFixture = AutoFixtureBuilder.FixtureBuild();

    public static RequestBreed BreedBuild()
    {
        return _autoFixture
            .Build<RequestBreed>()
            .Create();
    }

    public static IEnumerable<RequestBreed> BreedBuild(int countBreed)
    {
        var requestBreed = new List<RequestBreed>();

        for (var i = 1; i <= countBreed; i++)
        {
            requestBreed.Add(BreedBuild());
        }

        return requestBreed;
    }
}