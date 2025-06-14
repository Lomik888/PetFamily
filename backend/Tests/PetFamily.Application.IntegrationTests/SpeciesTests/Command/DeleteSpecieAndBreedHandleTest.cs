using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Specieses.Application.Commands.Delete;


namespace PetFamily.Application.IntegrationTests.SpeciesTests.Command;

public class DeleteSpecieAndBreedHandleTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 0;
    private const int COUNT_VOLUNTEERS_MIN = 1;
    private const int COUNT_PETS_MAX = 0;
    private const int COUNT_PETS_MIN = 1;
    private const int COUNT_SPECIES_MAX = 5;
    private const int COUNT_SPECIES_MIN = 2;
    private const int COUNT_BREEDS_MAX = 5;
    private const int COUNT_BREEDS_MIN = 2;
    private ICommandHandler<ErrorList, DeleteSpecieAndBreedCommand> _sut;

    public DeleteSpecieAndBreedHandleTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, DeleteSpecieAndBreedCommand>>();
    }

    [Fact]
    public async Task Delete_specie_and_breed_handle_Result_should_be_true_and_db_valid_version()
    {
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
        var cancellationToken = new CancellationToken();

        var pet = volunteers.First().Pets.First();
        var specie = species.First(x => x.Id.Value != pet.SpeciesBreedId.SpeciesId);
        var breed = specie.Breeds.First();

        var command = new DeleteSpecieAndBreedCommand(specie.Id.Value, breed.Id.Value);

        var result = await _sut.Handle(command, cancellationToken);

        var specieNotExists = SpeciesDbContext.Species.Any(x => x.Id == specie.Id);

        result.IsSuccess.Should().BeTrue();
        specieNotExists.Should().BeFalse();
    }
}