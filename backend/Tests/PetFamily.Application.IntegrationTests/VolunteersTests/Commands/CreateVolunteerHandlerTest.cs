using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Application.VolunteerUseCases.Commands.Create;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class CreateVolunteerHandlerTest : TestsBase
{
    private ICommandHandler<Guid, ErrorList, CreateVolunteerCommand> _sut;

    public CreateVolunteerHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<Guid, ErrorList, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Create_volunteer_handle_Result_should_be_true_and_valid_entity_in_db()
    {
        var cancellationToken = new CancellationToken();
        var request = RequestVolunteerBuilder.VolunteerBuild();
        var volunteer = VolunteerFactory.CreateVolunteer(request);
        var command = new CreateVolunteerCommand(
            new VolunteerNameDto(request.FirstName, request.LastName, request.Surname),
            request.Email,
            request.Description,
            request.Experience,
            new PhoneNumberDto(request.RegionCode, request.Number)
        );

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerId = VolunteerId.Create(result.Value).Value;
        var volunteerFromDb = await DbContext.Volunteers
            .Include(x => x.Pets)
            .SingleAsync(x => x.Id == volunteerId, default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Name.Should().BeEquivalentTo(volunteer.Name);
        volunteerFromDb.Email.Should().Be(volunteer.Email);
        volunteerFromDb.Description.Should().Be(volunteer.Description);
        volunteerFromDb.Experience.Should().Be(volunteer.Experience);
        volunteerFromDb.PhoneNumber.Should().Be(volunteer.PhoneNumber);
        volunteerFromDb.SocialNetworks.Items.Should().BeEquivalentTo(volunteer.SocialNetworks.Items);
        volunteerFromDb.DetailsForHelps.Items.Should().BeEquivalentTo(volunteer.DetailsForHelps.Items);
        volunteerFromDb.Files.Items.Should().BeEquivalentTo(volunteer.Files.Items);
        volunteerFromDb.Pets.Should().BeEquivalentTo(volunteer.Pets);
    }
}