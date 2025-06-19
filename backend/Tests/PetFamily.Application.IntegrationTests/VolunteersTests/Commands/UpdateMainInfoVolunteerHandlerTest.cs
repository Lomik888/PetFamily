using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Commands.UpdateMainInfo;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class UpdateMainInfoVolunteerHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS = 1;
    private ICommandHandler<Guid, ErrorList, UpdateMainInfoVolunteerCommand> _sut;

    public UpdateMainInfoVolunteerHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, ErrorList, UpdateMainInfoVolunteerCommand>>();
    }

    [Fact]
    public async Task
        Update_main_info_volunteer_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
    {
        var cancellationToken = new CancellationToken();
        var volunteers = await DomainSeedFactory.SeedVolunteersWithOutPetsAsync(
            VolunteerDbContext,
            COUNT_VOLUNTEERS);

        var volunteer = volunteers.Single();

        var description = Guid.NewGuid().ToString();
        var descriptionVo = Description.Create(description).Value;

        var command = new UpdateMainInfoVolunteerCommand(volunteer.Id.Value, null, description, null);

        //volunteer.UpdateMainInfo(volunteer.Name, descriptionVo, volunteer.Experience);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await VolunteerDbContext.Volunteers
            .Where(x => x.Id == volunteer.Id)
            .SingleAsync(default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().BeEquivalentTo(volunteer);
    }
}