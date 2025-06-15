using AutoFixture;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.UpdateStatusPet;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class UpdateStatusPetHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 0;
    private const int COUNT_VOLUNTEERS_MIN = 1;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 0;
    private const int COUNT_SPECIES_MIN = 1;
    private const int COUNT_BREEDS_MAX = 0;
    private const int COUNT_BREEDS_MIN = 1;
    private ICommandHandler<ErrorList, UpdateStatusPetCommand> _sut;

    public UpdateStatusPetHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, UpdateStatusPetCommand>>();
    }

    [Fact]
    public async Task
        Update_status_pet_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
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

        var volunteer = volunteers.Single();
        HelpStatuses status;
        Pet? pet;

        do
        {
            status = Fixture.Create<HelpStatuses>();
            pet = volunteer.Pets.FirstOrDefault(x => x.HelpStatus.Value != status);
        } while (pet == null);

        var helpStatus = HelpStatus.Create(status).Value;

        volunteer.SetPetStatus(pet, helpStatus);

        var command = new UpdateStatusPetCommand(volunteer.Id.Value, pet.Id.Value, status);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await VolunteerDbContext.Volunteers
            .Where(x => x.Id == volunteer.Id)
            .Include(x => x.Pets)
            .SingleAsync(default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().BeEquivalentTo(volunteer);
    }
}