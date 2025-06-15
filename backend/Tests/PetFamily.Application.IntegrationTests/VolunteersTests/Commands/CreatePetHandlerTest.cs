using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.CreatePet;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;

namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class CreatePetHandlerTest : TestsBase
{
    private const int COUNT_SPECIES = 1;
    private const int COUNT_BREEDS = 1;
    private const int COUNT_VOLUNTEERS_ = 1;
    private const int COUNT_PET = 1;
    private ICommandHandler<ErrorList, CreatePetCommand> _sut;

    public CreatePetHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, CreatePetCommand>>();
    }

    [Fact]
    public async Task Create_pet_handle_Result_should_be_true_and_valid_entity_in_db()
    {
        var speciesList =
            await DomainSeedFactory.SeedSpeciesWithBreedsAsync(SpeciesDbContext, COUNT_SPECIES, COUNT_BREEDS);
        var species = speciesList.Single();

        var petRequest = RequestPetBuilder.PetBuild(species.Id.Value, species.Breeds.First().Id.Value);

        var volunteerList =
            await DomainSeedFactory.SeedVolunteersWithOutPetsAsync(VolunteerDbContext, COUNT_VOLUNTEERS_);
        var volunteer = volunteerList.Single();
        var cancellationToken = new CancellationToken();

        IEnumerable<DetailsForHelpDto> detailsForHelps = [];

        var command = new CreatePetCommand(
            volunteer.Id.Value,
            petRequest.Name,
            new SpeciesBreedIdDto(petRequest.SpeciesId, petRequest.BreedId),
            petRequest.Age,
            petRequest.Description,
            petRequest.Color,
            new HealthDescriptionDto(
                petRequest.SharedHealthStatus,
                petRequest.SkinCondition,
                petRequest.MouthCondition,
                petRequest.DigestiveSystemCondition),
            new AddressDto(
                petRequest.Country,
                petRequest.City,
                petRequest.Street,
                petRequest.HouseNumber,
                petRequest.ApartmentNumber),
            petRequest.Weight,
            petRequest.Height,
            new PhoneNumberDto(petRequest.RegionCode, petRequest.Number),
            petRequest.Sterilize,
            petRequest.DateOfBirth,
            petRequest.Vaccinated,
            petRequest.HelpStatus,
            detailsForHelps
        );

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerWithPetFromDb = await VolunteerDbContext.Volunteers
            .Where(x => x.Id == volunteer.Id)
            .Include(x => x.Pets)
            .SingleAsync(default);

        result.IsSuccess.Should().BeTrue();
        volunteer.Should().BeEquivalentTo(volunteerWithPetFromDb);
    }
}