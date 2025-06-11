using PetFamily.Data.Tests.Requests;
using PetFamily.Specieses.Domain;
using PetFamily.Specieses.Domain.Ids;
using PetFamily.Specieses.Domain.SharedValueObjects;

namespace PetFamily.Data.Tests.Factories;

public static class BreedsFactory
{
    public static Species CreateBreed(Species species, RequestBreed requestBreed)
    {
        var breeds = new List<Breed>();
        var breed = new Breed(BreedId.Create(requestBreed.BreedId).Value,
            Name.Create(requestBreed.Name).Value);
        breeds.Add(breed);

        species.SetBreeds(breeds);
        return species;
    }

    public static Species CreateBreeds(Species species, IEnumerable<RequestBreed> requestBreeds)
    {
        foreach (var breed in requestBreeds)
        {
            CreateBreed(species, breed);
        }

        return species;
    }
}