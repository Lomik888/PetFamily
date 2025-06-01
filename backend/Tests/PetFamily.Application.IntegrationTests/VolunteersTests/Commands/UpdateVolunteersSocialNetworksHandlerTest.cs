using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateSocialNetworks;
using PetFamily.Data.Tests.Factories;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.IntegrationTests.VolunteersTests.Commands;

public class UpdateVolunteersSocialNetworksHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 0;
    private const int COUNT_VOLUNTEERS_MIN = 1;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 0;
    private const int COUNT_SPECIES_MIN = 1;
    private const int COUNT_BREEDS_MAX = 0;
    private const int COUNT_BREEDS_MIN = 1;
    private ICommandHandler<ErrorList, UpdateVolunteersSocialNetworksCommand> _sut;

    public UpdateVolunteersSocialNetworksHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<ICommandHandler<ErrorList, UpdateVolunteersSocialNetworksCommand>>();
    }

    [Fact]
    public async Task
        Update_volunteers_social_networks_handle_Result_should_be_true_and_volunteer_is_activated_and_volunteer_is_valid()
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

        var socialDomainIndex = Random.Next(0, SocialNetwork.Domains.Count);

        var socialNetworkDto = new SocialNetworkDto(SocialNetwork.Domains[socialDomainIndex],
            $"{SocialNetwork.Domains[socialDomainIndex]}/account");

        var socialNetworkCollectionDto = new SocialNetworkCollectionDto([socialNetworkDto]);

        var command = new UpdateVolunteersSocialNetworksCommand(volunteer.Id.Value, socialNetworkCollectionDto);

        var socialNetwork = SocialNetwork.Create(socialNetworkDto.Title, socialNetworkDto.Url).Value;
        var socialNetworks = SocialNetworks.Create([socialNetwork]).Value;
        volunteer.SetSocialNetworks(socialNetworks);

        var result = await _sut.Handle(command, cancellationToken);

        var volunteerFromDb = await DbContext.Volunteers
            .Where(x => x.Id == volunteer.Id)
            .Include(x => x.Pets)
            .SingleAsync(default);

        result.IsSuccess.Should().BeTrue();
        volunteerFromDb.Should().BeEquivalentTo(volunteer);
    }
}