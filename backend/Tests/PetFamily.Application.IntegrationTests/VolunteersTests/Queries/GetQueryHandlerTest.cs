using System.Text.Json;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.Core;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;
using PetFamily.Volunteers.Application.Queries.Get;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Queries;

public class GetQueryHandlerTest : TestsBase
{
    private const int PAGE = 2;
    private const int PAGESIZE = 3;
    private const int COUNT_VOLUNTEERS = 5;
    private IQueryHandler<GetObjectsWithPaginationResponse<VolunteerDto>, ErrorList, GetQuery> _sut;

    public GetQueryHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<GetObjectsWithPaginationResponse<VolunteerDto>, ErrorList, GetQuery>>();
    }

    [Fact]
    public async Task Get_query_handle_Result_should_be_true_and_valid_response()
    {
        var cancellationToken = new CancellationToken();

        await DomainSeedFactory.SeedVolunteersWithOutPetsAsync(VolunteerDbContext, COUNT_VOLUNTEERS);

        var query = new GetQuery(PAGE, PAGESIZE);

        using var connection = SqlConnectionFactory.Create();

        var parameters = new DynamicParameters()
            .AddPagination(query.Page, query.PageSize);

        var sql = $"""
                   select count(*) from "Volunteers".volunteers;

                   select v.id            as Id,
                          v.first_name      as FirstName,
                          v.last_name       as LastName,
                          v.surname         as Surname,
                          v.experience      as Experience,
                          v.social_networks as SocialNetworks,
                          COUNT(p.id)       as PetCount
                   from "Volunteers".volunteers as v
                            left join "Volunteers".pets as p on v.id = p.volunteer_id
                   GROUP BY v.id,
                            v.first_name,
                            v.last_name,
                            v.surname,
                            v.experience,
                            v.social_networks
                   offset @offset
                   limit @limit
                   """;

        await using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var volunteersCount = await multi.ReadFirstAsync<long>();
        var volunteersDtos = multi.Read<VolunteerDto, string, VolunteerDto>(
            (volunteerDto, socialsJson) =>
            {
                var socials = JsonSerializer.Deserialize<SocialNetworkDto[]>(socialsJson);
                volunteerDto.SocialNetworks = socials;
                return volunteerDto;
            },
            splitOn: "SocialNetworks"
        );

        var getObjectsWithPaginationResponse = new GetObjectsWithPaginationResponse<VolunteerDto>()
        {
            Data = volunteersDtos.ToArray(),
            PageSize = query.PageSize,
            Page = query.Page,
            Count = volunteersCount
        };

        var result = await _sut.Handle(query, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(getObjectsWithPaginationResponse);
    }
}