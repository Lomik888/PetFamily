using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.Delete;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class DeleteVolunteerHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 5;
    private const int COUNT_VOLUNTEERS_MIN = 2;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 5;
    private const int COUNT_SPECIES_MIN = 2;
    private const int COUNT_BREEDS_MAX = 5;
    private const int COUNT_BREEDS_MIN = 2;
    private ICommandHandler<ErrorList, DeleteVolunteerCommand> _sut;

    public DeleteVolunteerHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, DeleteVolunteerCommand>>();
    }

    [Fact]
    public async Task
        Hard_delete_volunteer_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
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
        var volunteerPetsCount = volunteer.Pets.Count;
        using var connection = SqlConnectionFactory.Create();
        var sql = $"""select count(*) from "Volunteers".pets;""";
        var petCountForEqual = await connection.QuerySingleAsync<long>(sql);

        var command = new DeleteVolunteerCommand(volunteer.Id.Value, DeleteType.HARD);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteersCountAfterHandle = await VolunteerDbContext.Volunteers.CountAsync(default);

        var petCountFromDb = await connection.QuerySingleAsync<long>(sql);

        result.IsSuccess.Should().BeTrue();
        volunteersCountAfterHandle.Should().Be(volunteersCount - 1);
        petCountFromDb.Should().Be(petCountForEqual - volunteerPetsCount);
    }

    [Fact]
    public async Task
        Soft_delete_volunteer_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
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
        using var connection = SqlConnectionFactory.Create();
        var sql = $"""select count(*) from "Volunteers".pets;""";
        var petCountForEqual = await connection.QuerySingleAsync<long>(sql);

        volunteer.UnActivate();

        var command = new DeleteVolunteerCommand(volunteer.Id.Value, DeleteType.SOFT);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteersCountAfterHandle = await VolunteerDbContext.Volunteers.CountAsync(default);
        var volunteerFromDb = await VolunteerDbContext.Volunteers.SingleAsync(x => x.Id == volunteer.Id, default);
        var petCountFromDb = await connection.QuerySingleAsync<long>(sql);

        result.IsSuccess.Should().BeTrue();
        volunteersCountAfterHandle.Should().Be(volunteersCount);
        petCountFromDb.Should().Be(petCountForEqual);
        volunteerFromDb.Should().BeEquivalentTo(volunteer, options => options.Excluding(x => x.DeletedAt));
    }
}