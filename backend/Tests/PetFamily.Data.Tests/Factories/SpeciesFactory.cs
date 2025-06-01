using PetFamily.Data.Tests.Requests;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.Ids;
using PetFamily.Domain.SpeciesContext.SharedVO;

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