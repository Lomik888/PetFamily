using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Extensions;
using PetFamily.Application.SpeciesUseCases.Queries.GetBreeds;
using PetFamily.Data.Tests.Factories;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.IntegrationTests.SpeciesTests.Queries;

public class GetBreedsQueryHandlerTest : TestsBase
{
    private const int COUNT_SPECIES = 3;
    private const int COUNT_BREEDS = 5;
    private const int PAGE = 3;
    private const int PAGESIZE = 3;
    private IQueryHandler<GetObjectsWithPaginationResponse<BreedsDto>, ErrorList, GetBreedsQuery> _sut;

    public GetBreedsQueryHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<
                IQueryHandler<GetObjectsWithPaginationResponse<BreedsDto>, ErrorList, GetBreedsQuery>>();
    }

    [Fact]
    public async Task Get_breeds_query_handle_result_should_be_true_and_valid_response()
    {
        var cancellationToken = new CancellationToken();
        var species = await DomainSeedFactory.SeedSpeciesWithBreedsAsync(
            DbContext,
            COUNT_SPECIES,
            COUNT_BREEDS);
        
        var speciesIndex = Random.Next(0, COUNT_SPECIES);
        var specie = species[speciesIndex];

        var request = new GetBreedsQuery(specie.Id.Value, PAGE, PAGESIZE);

        using var connection = SqlConnectionFactory.Create();

        var parameters = new DynamicParameters().AddPagination(request.Page, request.PageSize);
        parameters.Add("@speciesId", request.SpeciesId);

        var sql = $"""
                   select count(*) from breeds where species_id = @speciesId;                  

                   select
                       id as Id, 
                       name as Name 
                   from breeds
                   where species_id = @speciesId
                   offset @offset
                   limit @limit
                   """;

        await using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var speciesCount = await multi.ReadFirstAsync<long>();
        var speciesDtos = await multi.ReadAsync<BreedsDto>();

        var getObjectsWithPaginationResponse = new GetObjectsWithPaginationResponse<BreedsDto>()
        {
            Count = speciesCount,
            Page = request.Page,
            PageSize = request.PageSize,
            Data = speciesDtos,
        };

        var result = await _sut.Handle(request, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(getObjectsWithPaginationResponse);
    }
}