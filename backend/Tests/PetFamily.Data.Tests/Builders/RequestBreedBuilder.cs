using AutoFixture;
using PetFamily.Data.Tests.Requests;

namespace PetFamily.Data.Tests.Builders;

public static class RequestBreedBuilder
{
    private static readonly Fixture _autoFixture = new Fixture();
    private static readonly Random _random = new Random();

    public static RequestBreed BreedBuild()
    {
        var number = _random.Next(1, 100);

        return _autoFixture
            .Build<RequestBreed>()
            .With(x => x.Name, number.ToString())
            .Create();
    }

    public static IEnumerable<RequestBreed> BreedsBuild(int countBreed)
    {
        var requestsBreeds = new List<RequestBreed>();

        for (var i = 0; i < countBreed; i++)
        {
            requestsBreeds.Add(BreedBuild());
        }

        return requestsBreeds;
    }
}