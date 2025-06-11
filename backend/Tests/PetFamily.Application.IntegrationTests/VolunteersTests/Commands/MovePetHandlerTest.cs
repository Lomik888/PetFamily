using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.MovePet;
using PetFamily.Data.Tests.Factories;
using PetFamily.Domain.VolunteerContext.PetsVO;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class MovePetHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 0;
    private const int COUNT_VOLUNTEERS_MIN = 1;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 0;
    private const int COUNT_SPECIES_MIN = 1;
    private const int COUNT_BREEDS_MAX = 0;
    private const int COUNT_BREEDS_MIN = 1;
    private ICommandHandler<ErrorList, MovePetCommand> _sut;

    public MovePetHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, MovePetCommand>>();
    }

    [Fact]
    public async Task
        Move_pet_handle_Result_should_be_true_and_volunteer_is_valid()
    {
        var cancellationToken = new CancellationToken();
        var (volunteers, species) = await DomainSeedFactory.SeedFullModelsAsync(
            DbContext,
            COUNT_VOLUNTEERS_MIN,
            COUNT_VOLUNTEERS_MAX,
            COUNT_PETS_MIN,
            COUNT_PETS_MAX,
            COUNT_SPECIES_MIN,
            COUNT_SPECIES_MAX,
            COUNT_BREEDS_MIN,
            COUNT_BREEDS_MAX);

        var volunteer = volunteers.Single();
        var volunteerPetsCount = volunteer.Pets.Count;
        var serialNumber = (uint)Random.Next(1, volunteerPetsCount + 1);
        var petIndex = Random.Next(0, volunteerPetsCount);
        var pet = volunteer.Pets[petIndex];

        var serialNumberVo = SerialNumber.Create(serialNumber).Value;
        volunteer.MovePet(pet, serialNumberVo);

        var command = new MovePetCommand(serialNumber, pet.Id.Value, volunteer.Id.Value);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await DbContext.Volunteers
            .Include(x => x.Pets)
            .SingleAsync(x => x.Id == volunteer.Id, default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().Be(volunteer);
    }
}