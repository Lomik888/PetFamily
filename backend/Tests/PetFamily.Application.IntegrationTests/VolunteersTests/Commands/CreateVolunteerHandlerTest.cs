using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Commands.Create;
using PetFamily.Shared.Errors;

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
    public async Task Create_volunteer_handle_Result_should_be_true()
    {
        var cancellationToken = new CancellationToken();
        var command = Fixture.Build<CreateVolunteerCommand>()
            .With(x => x.Email, "asdasd@gmail.com")
            .With(x => x.Phone, new PhoneNumberDto("+7", "1231231243"))
            .Create();

        var result = await _sut.Handle(command, cancellationToken);

        result.IsSuccess.Should().BeTrue();
    }
}