using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.Delete;
using PetFamily.Volunteers.Application.Commands.DeletePet;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class DeletePetHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 5;
    private const int COUNT_VOLUNTEERS_MIN = 2;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 5;
    private const int COUNT_SPECIES_MIN = 2;
    private const int COUNT_BREEDS_MAX = 5;
    private const int COUNT_BREEDS_MIN = 2;
    private ICommandHandler<ErrorList, DeletePetCommand> _sut;

    public DeletePetHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, DeletePetCommand>>();
    }

    [Fact]
    public async Task
        Hard_delete_pet_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
    {
        var vols = VolunteerDbContext.Volunteers.ToList();
        var cancellationToken = new CancellationToken();
        var (volunteers, species) = await DomainSeedFactory.SeedFullModelsAsync(
            TestDbContext,
            COUNT_VOLUNTEERS_MIN,
            COUNT_VOLUNTEERS_MAX,
            COUNT_PETS_MIN,
            COUNT_PETS_MAX,
            COUNT_SPECIES_MIN,
            COUNT_SPECIES_MAX,
            COUNT_BREEDS_MIN,
            COUNT_BREEDS_MAX);

        var volunteersCount = volunteers.Count;
        var volunteerIndex = Random.Next(0, volunteersCount);
        var volunteer = volunteers[volunteerIndex];
        var petIndex = Random.Next(0, volunteer.Pets.Count);
        var pet = volunteer.Pets[petIndex];

        var volunteerPetsCount = volunteer.Pets.Count;
        using var connection = SqlConnectionFactory.Create();
        var sql = $"""select count(*) from "Volunteers".pets;""";
        var petCountForEqual = await connection.QuerySingleAsync<long>(sql);

        var command = new DeletePetCommand(volunteer.Id.Value, pet.Id.Value, DeleteType.HARD);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteersCountAfterHandle = await VolunteerDbContext.Volunteers.CountAsync(default);
        var volunteerFromDb = await VolunteerDbContext.Volunteers
            .Include(x => x.Pets)
            .SingleAsync(x => x.Id == volunteer.Id, default);
        var petCountFromDb = await connection.QuerySingleAsync<long>(sql);

        result.IsSuccess.Should().BeTrue();
        volunteersCountAfterHandle.Should().Be(volunteersCount);
        petCountFromDb.Should().Be(petCountForEqual - 1);
        volunteerFromDb.Pets.Count.Should().Be(volunteerPetsCount - 1);
    }

    [Fact]
    public async Task
        Soft_delete_pet_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
    {
        var cancellationToken = new CancellationToken();
        var (volunteers, species) = await DomainSeedFactory.SeedFullModelsAsync(
            TestDbContext,
            COUNT_VOLUNTEERS_MIN,
            COUNT_VOLUNTEERS_MAX,
            COUNT_PETS_MIN,
            COUNT_PETS_MAX,
            COUNT_SPECIES_MIN,
            COUNT_SPECIES_MAX,
            COUNT_BREEDS_MIN,
            COUNT_BREEDS_MAX);

        var volunteersCount = volunteers.Count;
        var volunteerIndex = Random.Next(0, volunteersCount);
        var volunteer = volunteers[volunteerIndex];
        var petIndex = Random.Next(0, volunteer.Pets.Count);
        var pet = volunteer.Pets[petIndex];

        using var connection = SqlConnectionFactory.Create();
        var sql = $"""select count(*) from "Volunteers".pets;""";
        var petCountForEqual = await connection.QuerySingleAsync<long>(sql);

        volunteer.UnActivatePet(pet);

        var command = new DeletePetCommand(volunteer.Id.Value, pet.Id.Value, DeleteType.SOFT);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteersCountAfterHandle = await VolunteerDbContext.Volunteers.CountAsync(default);
        var petFromDb = await VolunteerDbContext.Volunteers
            .Where(x => x.Id == volunteer.Id)
            .Include(x => x.Pets.Where(x => x.Id == pet.Id))
            .Select(x => new { Pet = x.Pets.Single(x => x.Id == pet.Id) })
            .SingleAsync(default);
        var petCountFromDb = await connection.QuerySingleAsync<long>(sql);

        result.IsSuccess.Should().BeTrue();
        volunteersCountAfterHandle.Should().Be(volunteersCount);
        petCountFromDb.Should().Be(petCountForEqual);
        petFromDb.Pet.Should().BeEquivalentTo(pet, options => options.Excluding(x => x.DeletedAt));
    }
}