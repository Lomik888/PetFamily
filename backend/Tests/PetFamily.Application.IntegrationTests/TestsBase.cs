using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Repositories;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.Ids;
using PetFamily.Domain.SpeciesContext.SharedVO;
using PetFamily.Domain.UnitTests.VolunteerTests.Builders;
using PetFamily.Domain.UnitTests.VolunteerTests.Fixtures;
using PetFamily.Domain.UnitTests.VolunteerTests.Requests;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Infrastructure.DbContext.PostgresSQL;

namespace PetFamily.Application.IntegrationTests;

public class TestsBase : IClassFixture<IntegrationsTestsWebAppFactory>, IAsyncLifetime
{
    protected readonly IntegrationsTestsWebAppFactory Factory;
    protected readonly IServiceScope Scope;
    protected readonly ApplicationDbContext DbContext;
    protected readonly IFixture Fixture;

    private readonly Func<Task> _resetDatabase;

    protected TestsBase(IntegrationsTestsWebAppFactory factory)
    {
        Factory = factory;
        _resetDatabase = factory.ResetDatabaseAsync;
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Fixture = new Fixture();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await _resetDatabase();
    }

    public async Task<List<Volunteer>> SeedVolunteersWithOutPetsAsync(int count)
    {
        var volunteers = new List<Volunteer>();

        for (int i = 0; i < count; i++)
        {
            var volunteerRequest = RequestVolunteerBuilder.VolunteerBuild();
            var volunteer = VolunteerFixture.CreateVolunteerWithOutPets(volunteerRequest);
            volunteers.Add(volunteer);
        }

        await DbContext.AddRangeAsync(volunteers);
        await DbContext.SaveChangesAsync();

        return volunteers;
    }

    public async Task<List<Volunteer>> SeedVolunteersWithPetsAsync(int countVolunteers, int countVolunteersPets)
    {
        var volunteers = new List<Volunteer>();

        for (int i = 0; i < countVolunteers; i++)
        {
            var volunteerRequest = RequestVolunteerBuilder.VolunteerBuild();
            var volunteer = VolunteerFixture.CreateVolunteerWithOutPets(volunteerRequest);
            var petsRequests = RequestPetBuilder.PetsBuild(countVolunteersPets);
            VolunteerFixture.CreatePets(volunteer, petsRequests);
            volunteers.Add(volunteer);
        }

        await DbContext.AddRangeAsync(volunteers);
        await DbContext.SaveChangesAsync();

        return volunteers;
    }

    public async Task<List<Species>> SeedSpeciesWithBreedsAsync(int speciesCount, int breedCount)
    {
        var random = new Random();

        var speciesRepository = Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        var speciesRequest = RequestSpeciesBuilder.SpeciesBuild(speciesCount);
        var breedRequest = RequestBreedBuilder.BreedBuild(breedCount);

        var species = speciesRequest
            .Select(x => new Species(SpeciesId.Create(x.SpeciesId).Value, Name.Create(x.Name).Value)).ToList();
        var breeds = breedRequest
            .Select(x => new Breed(BreedId.Create(x.BreedId).Value, Name.Create(x.Name).Value)).ToList();
        var breedsToSpecies = new List<Breed>(breeds);

        foreach (var specie in species)
        {
            var count = random.Next(speciesCount, breedCount + 1);
            var selectBreeds = breedsToSpecies.Take(count).ToList();
            breedsToSpecies = breedsToSpecies.Except(selectBreeds).ToList();
            specie.SetBreeds(breedsToSpecies);
        }

        await speciesRepository.AddRangeAsync(species);

        return species;
    }

    public async Task<(List<Volunteer>, List<Species>)> SeedFullModelsAsync(
        int countVolunteers,
        int countVolunteersPets,
        int speciesCount,
        int breedCount)
    {
        var random = new Random();

        var volunteers = new List<Volunteer>();
        var species = await SeedSpeciesWithBreedsAsync(speciesCount, breedCount);

        for (int i = 0; i < countVolunteers; i++)
        {
            var petsRequests = new List<RequestPet>();
            var volunteerRequest = RequestVolunteerBuilder.VolunteerBuild();
            var volunteer = VolunteerFixture.CreateVolunteerWithOutPets(volunteerRequest);

            for (int j = 0; j < countVolunteersPets; j++)
            {
                var indexSpecie = random.Next(0, breedCount + 1);
                var specie = species[indexSpecie];

                var indexBreed = random.Next(0, specie.Breeds.Count + 1);
                var breed = specie.Breeds[indexBreed];

                var petRequest = RequestPetBuilder.PetBuild(specie.Id.Value, breed.Id.Value);
                petsRequests.Add(petRequest);
            }

            VolunteerFixture.CreatePets(volunteer, petsRequests);
            volunteers.Add(volunteer);
        }

        await DbContext.AddRangeAsync(volunteers);
        await DbContext.SaveChangesAsync();

        return (volunteers, species);
    }
}