using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Specieses.Application.Dtos;

namespace PetFamily.Specieses.Application.Queries.GetBreeds;

public class GetBreedsQueryHandler
    : IQueryHandler<GetObjectsWithPaginationResponse<BreedsDto>, ErrorList, GetBreedsQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetBreedsQueryHandler> _logger;
    private readonly IValidator<GetBreedsQuery> _validator;

    public GetBreedsQueryHandler(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetBreedsQueryHandler> logger,
        IValidator<GetBreedsQuery> validator)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<GetObjectsWithPaginationResponse<BreedsDto>, ErrorList>> Handle(
        GetBreedsQuery request,
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
        parameters.Add("@speciesId", request.SpeciesId);

        var sql = $"""
                   select count(*) from "Species".breeds where species_id = @speciesId;                  

                   select
                       id as Id, 
                       name as Name 
                   from "Species".breeds
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

        return getObjectsWithPaginationResponse;
    }
}