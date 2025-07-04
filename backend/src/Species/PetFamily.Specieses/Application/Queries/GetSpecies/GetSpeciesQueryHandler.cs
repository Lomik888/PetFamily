﻿using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Specieses.Application.Dtos;

namespace PetFamily.Specieses.Application.Queries.GetSpecies;

public class
    GetSpeciesQueryHandler : IQueryHandler<GetObjectsWithPaginationResponse<SpeciesDto>, ErrorList, GetSpeciesQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetSpeciesQueryHandler> _logger;
    private readonly IValidator<GetSpeciesQuery> _validator;

    public GetSpeciesQueryHandler(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetSpeciesQueryHandler> logger,
        IValidator<GetSpeciesQuery> validator)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<GetObjectsWithPaginationResponse<SpeciesDto>, ErrorList>> Handle(
        GetSpeciesQuery request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        using var connection = _connectionFactory.Create();

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

        return getObjectsWithPaginationResponse;
    }
}