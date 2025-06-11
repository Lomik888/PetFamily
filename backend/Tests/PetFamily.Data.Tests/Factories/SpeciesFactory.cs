using PetFamily.Data.Tests.Requests;
using PetFamily.Specieses.Domain;
using PetFamily.Specieses.Domain.Ids;
using PetFamily.Specieses.Domain.SharedValueObjects;

namespace PetFamily.Data.Tests.Factories;

public class SpeciesFactory
{
    public static Species CreateSpecies(RequestSpecies requestSpecies)
    {
        var speciesId = SpeciesId.Create(requestSpecies.SpeciesId).Value;
        var name = Name.Create(requestSpecies.Name).Value;
        var specie = new Species(speciesId, name);

        return specie;
    }

    public static IEnumerable<Species> CreateSpecies(IEnumerable<RequestSpecies> requestsSpecies)
    {
        var species = new List<Species>();

        foreach (var request in requestsSpecies)
        {
            var specie = CreateSpecies(request);
            species.Add(specie);
        }

        return species;
    }
}