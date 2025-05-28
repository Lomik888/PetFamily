using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts.DTO;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Queries.Get;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.IntegrationTests.VolunteersTests.Queries;

public class GetHandlerTest : TestsBase
{
    private const int PAGE = 2;
    private const int PAGESIZE = 3;
    private const int COUNTVOLUNTEERS = 5;
    private IQueryHandler<GetObjectsWithPaginationResponse<VolunteerDto>, ErrorList, GetQuery> _sut;

    public GetHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<GetObjectsWithPaginationResponse<VolunteerDto>, ErrorList, GetQuery>>();
    }

    [Fact]
    public async Task Get_handle_Result_should_be_true_and_valid_response()
    {
        var cancellationToken = new CancellationToken();
        var volunteers = await SeedVolunteersWithOutPetsAsync(COUNTVOLUNTEERS);
        var query = new GetQuery(PAGE, PAGESIZE);
        var volunteersDtos = new List<VolunteerDto>();
        var handleResultSuccessValue = new GetObjectsWithPaginationResponse<VolunteerDto>()
        {
            Data = volunteersDtos,
            Page = PAGE,
            PageSize = PAGESIZE,
            Count = volunteers.Count,
        };

        foreach (var volunteer in volunteers.OrderBy(x => x.Id.Value).Skip((PAGE - 1) * PAGESIZE).Take(PAGESIZE))
        {
            var volunteerDto = new VolunteerDto();
            volunteerDto.Id = volunteer.Id.Value;
            volunteerDto.FirstName = volunteer.Name.FirstName;
            volunteerDto.LastName = volunteer.Name.LastName;
            volunteerDto.Surname = volunteer.Name.Surname;
            volunteerDto.Experience = volunteer.Experience.Value;
            volunteerDto.SocialNetworks = volunteer.SocialNetworks.Items
                .Select(x => new SocialNetworkDto(x.Title, x.Url)).ToArray();
            volunteerDto.PetCount = volunteer.Pets.Count;

            volunteersDtos.Add(volunteerDto);
        }

        var result = await _sut.Handle(query, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(handleResultSuccessValue);
    }
}