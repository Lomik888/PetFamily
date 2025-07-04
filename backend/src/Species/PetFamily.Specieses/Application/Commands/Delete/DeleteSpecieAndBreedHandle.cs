﻿using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Specieses.Application.Abstractions;
using PetFamily.Specieses.Domain.Ids;

namespace PetFamily.Specieses.Application.Commands.Delete;

public class DeleteSpecieAndBreedHandle : ICommandHandler<ErrorList, DeleteSpecieAndBreedCommand>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly IValidator<DeleteSpecieAndBreedCommand> _validator;
    private readonly ILogger<DeleteSpecieAndBreedHandle> _logger;
    private readonly ISpeciesRepository _speciesRepository;

    public DeleteSpecieAndBreedHandle(
        ISqlConnectionFactory connectionFactory,
        IValidator<DeleteSpecieAndBreedCommand> validator,
        ILogger<DeleteSpecieAndBreedHandle> logger,
        ISpeciesRepository speciesRepository)
    {
        _connectionFactory = connectionFactory;
        _validator = validator;
        _logger = logger;
        _speciesRepository = speciesRepository;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeleteSpecieAndBreedCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var parameters = new DynamicParameters();
        parameters.Add("@specieId", request.SpeciesId);
        parameters.Add("@breedId", request.BreedId);

        var result = await GetExistInfoAsync(parameters, cancellationToken);

        if (result.SpeciesAndBreedsExist == false && result.SpeciesExist == true)
        {
            return await BreedNotExist(request, cancellationToken);
        }

        if (result.SpeciesExist == false)
        {
            return await SpeciesNotExist(request, cancellationToken);
        }

        return UnitResult.Success<ErrorList>();
    }

    private async Task<ResultDto> GetExistInfoAsync(DynamicParameters parameters, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.Create();

        var sql = $"""
                   select 
                    exists(select 1 from "Volunteers".pets where species_id = @specieId)
                        as SpeciesExist,
                    exists(select 1 from "Volunteers".pets where species_id = @specieId and breed_id = @breedId)
                        as SpeciesAndBreedsExist
                   """;

        var result = await connection.QuerySingleAsync<ResultDto>(sql, parameters);

        return result;
    }

    private async Task<UnitResult<ErrorList>> BreedNotExist(
        DeleteSpecieAndBreedCommand request,
        CancellationToken cancellationToken)
    {
        var speciesId = SpeciesId.Create(request.SpeciesId).Value;
        var species = await _speciesRepository.GetSpeciesByIdWithBreedsAsync(speciesId, cancellationToken);
        var newBreeds = species.Breeds.Where(x => x.Id.Value != request.BreedId).ToList();
        species.SetBreeds(newBreeds);
        await _speciesRepository.UpdateAlreadyTrackingAsync(cancellationToken);
        return UnitResult.Success<ErrorList>();
    }

    private async Task<UnitResult<ErrorList>> SpeciesNotExist(
        DeleteSpecieAndBreedCommand request,
        CancellationToken cancellationToken)
    {
        var speciesId = SpeciesId.Create(request.SpeciesId).Value;
        var species = await _speciesRepository.GetSpeciesByIdAsync(speciesId, cancellationToken);
        await _speciesRepository.RemoveAsync(species, cancellationToken);
        return UnitResult.Success<ErrorList>();
    }

    private record ResultDto(bool SpeciesExist, bool SpeciesAndBreedsExist);
}