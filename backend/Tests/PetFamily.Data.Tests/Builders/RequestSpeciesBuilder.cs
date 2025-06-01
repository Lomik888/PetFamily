using AutoFixture;
using PetFamily.Data.Tests.Requests;

namespace PetFamily.Data.Tests.Builders;

public static class RequestSpeciesBuilder
{
    private static readonly Fixture _autoFixture = new Fixture();
    private static readonly Random _random = new Random();

    public static RequestSpecies SpeciesBuild()
    {
        var number = _random.Next(1, 100);

        return _autoFixture
            .Build<RequestSpecies>()
            .With(x => x.Name, number.ToString())
            .Create();
    }

    public static IEnumerable<RequestSpecies> SpeciesBuild(int countSpecies)
    {
        var requestSpecies = new List<RequestSpecies>();

        for (var i = 0; i < countSpecies; i++)
        {
            requestSpecies.Add(SpeciesBuild());
        }

        return requestSpecies;
    }
}