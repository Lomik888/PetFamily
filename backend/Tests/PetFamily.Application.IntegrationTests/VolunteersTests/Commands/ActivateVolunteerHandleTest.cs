using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.Activate;
using PetFamily.Data.Tests.Factories;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class ActivateVolunteerHandleTest : TestsBase
{
    private const int VOLUNTEERS_COUNT = 1;
    private ICommandHandler<ErrorList, ActivateVolunteerCommand> _sut;

    public ActivateVolunteerHandleTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, ActivateVolunteerCommand>>();
    }

    [Fact]
    public async Task
        Activate_volunteer_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
    {
        var volunteerForEqual = await DomainSeedFactory.SeedVolunteersWithOutPetsAsync(DbContext, VOLUNTEERS_COUNT);
        var volunteer = await DbContext.Volunteers.SingleAsync(x => x.Id == volunteerForEqual.First().Id);
        volunteer.UnActivate();
        await DbContext.SaveChangesAsync();
        var volunteerId = volunteer.Id.Value;

        var cancellationToken = new CancellationToken();
        var command = new ActivateVolunteerCommand(volunteerId);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerAfterHandle =
            await DbContext.Volunteers.SingleAsync(x => x.Id == volunteer.Id, default);

        result.IsSuccess.Should().BeTrue();
        volunteerAfterHandle.IsActive.Should().BeTrue();
        volunteerForEqual.Single().Should().BeEquivalentTo(volunteerAfterHandle);
    }
}