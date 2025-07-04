﻿using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.Core;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Specieses.Application.Dtos;
using PetFamily.Specieses.Application.Queries.GetSpecies;


namespace PetFamily.Application.IntegrationTests.SpeciesTests.Queries;

public class GetSpeciesQueryHandlerTest : TestsBase
{
    private const int COUNT_SPECIES = 3;
    private const int COUNT_BREEDS = 3;
    private const int PAGE = 3;
    private const int PAGESIZE = 3;
    private IQueryHandler<GetObjectsWithPaginationResponse<SpeciesDto>, ErrorList, GetSpeciesQuery> _sut;

    public GetSpeciesQueryHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<
                IQueryHandler<GetObjectsWithPaginationResponse<SpeciesDto>, ErrorList, GetSpeciesQuery>>();
    }

    [Fact]
    public async Task Get_species_query_handle_Result_should_be_true_and_valid_response()
    {
        var cancellationToken = new CancellationToken();
        var species = await DomainSeedFactory.SeedSpeciesWithBreedsAsync(
            SpeciesDbContext,
            COUNT_SPECIES,
            COUNT_BREEDS);

        var request = new GetSpeciesQuery(PAGE, PAGESIZE);

        using var connection = SqlConnectionFactory.Create();

        var parameters = new DynamicParameters().AddPagination(request.Page, request.PageSize);

        var sql = $"""
                   select count(*) from "Species".species;                  

                   select
                       id as Id, 
                       name as Name 
                   from "Species".species
                   offset @offset
                   limit @limit
                   """;

        await using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var speciesCount = await multi.ReadFirstAsync<long>();
        var speciesDtos = await multi.ReadAsync<SpeciesDto>();

        var getObjectsWithPaginationResponse = new GetObjectsWithPaginationResponse<SpeciesDto>()
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