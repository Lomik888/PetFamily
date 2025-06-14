using Microsoft.EntityFrameworkCore;
using PetFamily.Data.Tests.Builders;
using PetFamily.Specieses.Domain;
using PetFamily.Volunteers.Domain;

namespace PetFamily.Data.Tests.Factories;

public static class DomainSeedFactory
{
    private static readonly Random _random = new Random();

    public static async Task<List<Volunteer>> SeedVolunteersWithOutPetsAsync(DbContext dbContext, int count)
    {
        var volunteers = new List<Volunteer>();

        for (int i = 0; i < count; i++)
        {
            var volunteerRequest = RequestVolunteerBuilder.VolunteerBuild();
            var volunteer = VolunteerFactory.CreateVolunteer(volunteerRequest);
            volunteers.Add(volunteer);
        }

        await dbContext.AddRangeAsync(volunteers);
        await dbContext.SaveChangesAsync();

        return volunteers;
    }

    public static async Task<(List<Volunteer>, List<Species>)> SeedFullModelsAsync(
        DbContext testDbContext,
        int volunteersMinCount,
        int volunteersMaxCount,
        int petsMinCount,
        int petsMaxCount,
        int speciesMinCount,
        int speciesMaxCount,
        int breedsMinCount,
        int breedsMaxCount)
    {
        var (species, volunteers) = DomainFactory.CreateFullDomainModels(
            volunteersMinCount,
            volunteersMaxCount,
            petsMinCount,
            petsMaxCount,
            speciesMinCount,
            speciesMaxCount,
            breedsMinCount,
            breedsMaxCount);

        await testDbContext.AddRangeAsync(species);
        await testDbContext.AddRangeAsync(volunteers);
        await testDbContext.SaveChangesAsync();

        return (volunteers, species);
    }

    public static async Task<List<Species>> SeedSpeciesWithBreedsAsync(
        DbContext dbContext,
        int speciesCount,
        int breedCount)
    {
        var speciesRequest = RequestSpeciesBuilder.SpeciesBuild(speciesCount);
        var species = new List<Species>();

        foreach (var request in speciesRequest)
        {
            var breedRequest = RequestBreedBuilder.BreedsBuild(breedCount);
            var specie = SpeciesFactory.CreateSpecies(request);
            BreedsFactory.CreateBreeds(specie, breedRequest);
            species.Add(specie);
        }

        await dbContext.AddRangeAsync(species);
        await dbContext.SaveChangesAsync();

        return species;
    }
}