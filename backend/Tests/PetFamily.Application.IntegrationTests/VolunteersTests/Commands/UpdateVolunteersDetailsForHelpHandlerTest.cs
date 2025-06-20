using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Application.Commands.UpdateDetailsForHelps;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class UpdateVolunteersDetailsForHelpHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS = 1;
    private ICommandHandler<ErrorList, UpdateVolunteersDetailsForHelpCommand> _sut;

    public UpdateVolunteersDetailsForHelpHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, UpdateVolunteersDetailsForHelpCommand>>();
    }

    [Fact]
    public async Task
        Move_pet_handle_Result_should_be_true_and_volunteer_is_valid()
    {
        var cancellationToken = new CancellationToken();
        var volunteers = await DomainSeedFactory.SeedVolunteersWithOutPetsAsync(VolunteerDbContext, COUNT_VOLUNTEERS);

        var volunteer = volunteers.Single();

        var detailsForHelp = DetailsForHelp.Create("SomeTitle", "SomeDescription").Value;
        var detailsForHelpDto = new DetailsForHelpDto("SomeTitle", "SomeDescription");

        var detailsForHelps = DetailsForHelps.Create([detailsForHelp]).Value;
        //volunteer.SetDetailsForHelps(detailsForHelps);
        var command = new UpdateVolunteersDetailsForHelpCommand(volunteer.Id.Value,
            new DetailsForHelpCollectionDto([detailsForHelpDto]));

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await VolunteerDbContext.Volunteers
            .SingleAsync(x => x.Id == volunteer.Id, default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().Be(volunteer);
    }
}