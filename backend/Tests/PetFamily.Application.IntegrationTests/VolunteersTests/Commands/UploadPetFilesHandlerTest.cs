using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts.DTO.PetDtos;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.UploadPetFiles;
using PetFamily.Data.Tests.Factories;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class UploadPetFilesHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 0;
    private const int COUNT_VOLUNTEERS_MIN = 1;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 0;
    private const int COUNT_SPECIES_MIN = 1;
    private const int COUNT_BREEDS_MAX = 0;
    private const int COUNT_BREEDS_MIN = 1;
    private ICommandHandler<ErrorList, UploadPetFilesCommand> _sut;

    public UploadPetFilesHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, UploadPetFilesCommand>>();
    }

    [Fact]
    public async Task
        Move_pet_handle_Result_should_be_true_and_volunteer_is_valid()
    {
        Factory.FileProviderSuccessUploadAsync();

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

        var filesPet = FilesPet.Create([fileFirst]).Value;
        volunteer.SetPetFiles(pet, filesPet);

        var filesToDelete = FilesPet.Create([fileFirst]).Value;
        volunteer.RemovePetFiles(pet, filesToDelete);

        var fileInfoDto = new FileInfoDto(fileFirst.FullPath, ".png", 234);
        var stream = Stream.Null;
        var fileDto = new UploadFileDto(stream, fileInfoDto);

        var command = new UploadPetFilesCommand([fileDto], volunteer.Id.Value, pet.Id.Value);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await DbContext.Volunteers
            .Include(x => x.Pets)
            .SingleAsync(x => x.Id == volunteer.Id, default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().BeEquivalentTo(volunteer);
    }
}