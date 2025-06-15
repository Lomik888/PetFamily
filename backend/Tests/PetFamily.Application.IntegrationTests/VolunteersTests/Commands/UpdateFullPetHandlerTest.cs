using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.UpdateFullPet;
using PetFamily.Volunteers.Domain.Dtos;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class UpdateFullPetHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 0;
    private const int COUNT_VOLUNTEERS_MIN = 1;
    private const int COUNT_PETS_MAX = 0;
    private const int COUNT_PETS_MIN = 1;
    private const int COUNT_SPECIES_MAX = 0;
    private const int COUNT_SPECIES_MIN = 1;
    private const int COUNT_BREEDS_MAX = 0;
    private const int COUNT_BREEDS_MIN = 1;
    private ICommandHandler<ErrorList, UpdateFullPetCommand> _sut;

    public UpdateFullPetHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, UpdateFullPetCommand>>();
    }

    [Fact]
    public async Task
        Update_full_pet_handle_Result_should_be_true_and_volunteer_is_valid()
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
        var pet = volunteer.Pets.Single();

        var command = new UpdateFullPetCommand(
            volunteer.Id.Value,
            pet.Id.Value,
            "Дым Дмитрий",
            null,
            "SomeDesc",
            "Серый рыжий",
            null,
            null,
            5D,
            null,
            null,
            true,
            null,
            true,
            null,
            null,
            null);

        var description = Description.Create(command.Description!).Value;
        var nickName = NickName.Create(command.NickName!).Value;
        var color = Color.Create(command.Color!).Value;
        var weight = Weight.Create((double)command.Weight!).Value;

        var dto = new UpdatePetFullInfoDto(
            nickName,
            pet.SpeciesBreedId,
            description,
            color,
            pet.HealthDescription,
            pet.Address,
            weight,
            pet.Height,
            pet.PhoneNumber,
            (bool)command.Sterilize!,
            pet.DateOfBirth,
            (bool)command.Vaccinated!,
            pet.HelpStatus,
            pet.DetailsForHelps,
            pet.FilesPet
        );

        volunteer.UpdateFullInfoAboutPet(pet, dto);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await VolunteerDbContext.Volunteers
            .Include(x => x.Pets)
            .SingleAsync(x => x.Id == volunteer.Id, default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().BeEquivalentTo(volunteer);
    }
}