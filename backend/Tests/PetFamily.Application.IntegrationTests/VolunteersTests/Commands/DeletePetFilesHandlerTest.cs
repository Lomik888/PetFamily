using File = PetFamily.Domain.VolunteerContext.SharedVO.File;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Commands.DeletePetFiles;
using PetFamily.Data.Tests.Factories;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class DeletePetFilesHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 0;
    private const int COUNT_VOLUNTEERS_MIN = 1;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 0;
    private const int COUNT_SPECIES_MIN = 1;
    private const int COUNT_BREEDS_MAX = 0;
    private const int COUNT_BREEDS_MIN = 1;
    private ICommandHandler<ErrorList, DeletePetFilesCommand> _sut;

    public DeletePetFilesHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, DeletePetFilesCommand>>();
    }

    [Fact]
    public async Task
        Delete_pet_files_handle_Result_should_be_true_and_entity_is_valid_in_db()
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
        var petIndex = Random.Next(0, volunteerPetsCount);
        var pet = volunteer.Pets[petIndex];

        var fileFirst = File.Create("/1/file1/file1.png").Value;
        var fileSecond = File.Create("/2/file2/file2.png").Value;
        var fileThird = File.Create("/3/file3/file3.png").Value;

        var filesPet = FilesPet.Create([fileFirst, fileSecond, fileThird]).Value;
        volunteer.SetPetFiles(pet, filesPet);

        await DbContext.SaveChangesAsync(default);

        var filesToDelete = FilesPet.Create([fileFirst, fileThird]).Value;
        volunteer.RemovePetFiles(pet, filesToDelete);

        var command = new DeletePetFilesCommand([fileSecond.FullPath], volunteer.Id.Value, pet.Id.Value);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await DbContext.Volunteers
            .Include(x => x.Pets)
            .SingleAsync(x => x.Id == volunteer.Id, default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().BeEquivalentTo(volunteer);
    }
}