﻿using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

namespace PetFamily.Volunteers.Application.Queries.Get;

public class GetQueryHandler : IQueryHandler<GetObjectsWithPaginationResponse<VolunteerDto>, ErrorList, GetQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetQueryHandler> _logger;
    private readonly IValidator<GetQuery> _validator;

    public GetQueryHandler(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetQueryHandler> logger,
        IValidator<GetQuery> validator)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<GetObjectsWithPaginationResponse<VolunteerDto>, ErrorList>> Handle(
        GetQuery request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        using var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters()
            .AddPagination(request.Page, request.PageSize);

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
            PageSize = request.PageSize,
            Page = request.Page,
            Count = volunteersCount
        };

        return getObjectsWithPaginationResponse;
    }
}