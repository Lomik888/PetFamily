using PetFamily.Data.Tests.Builders;
using PetFamily.Specieses.Domain;
using PetFamily.Volunteers.Domain;

namespace PetFamily.Data.Tests.Factories;

public static class DomainFactory
{
    private static readonly Random _random = new Random();

    public static (List<Species> species, List<Volunteer> volunteer) CreateFullDomainModels(
        int volunteersMinCount,
        int volunteersMaxCount,
        int petsMinCount,
        int petsMaxCount,
        int speciesMinCount,
        int speciesMaxCount,
        int breedsMinCount,
        int breedsMaxCount)
    {
        var volunteersCount = _random.Next(volunteersMinCount, volunteersMaxCount + 1);
        var speciesCount = _random.Next(speciesMinCount, speciesMaxCount + 1);

        var volunteersRequests = RequestVolunteerBuilder.VolunteersBuild(volunteersCount);
        var speciesRequests = RequestSpeciesBuilder.SpeciesBuild(speciesCount);

        var volunteers = VolunteerFactory.CreateVolunteers(volunteersRequests).ToList();
        var species = SpeciesFactory.CreateSpecies(speciesRequests).ToList();

        foreach (var specie in species)
        {
            var breedsCount = _random.Next(breedsMinCount, breedsMaxCount + 1);
            var breedsRequests = RequestBreedBuilder.BreedsBuild(breedsCount);
            BreedsFactory.CreateBreeds(specie, breedsRequests);
        }

        foreach (var volunteer in volunteers)
        {
            var petsCount = _random.Next(petsMinCount, petsMaxCount + 1);
            var petsRequests = RequestPetBuilder.PetsBuild(species, petsCount);
            PetsFactory.CreatePets(volunteer, petsRequests);
        }

        return (species, volunteers);
    }
}