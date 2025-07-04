﻿using PetFamily.Specieses.Domain;
using PetFamily.Specieses.Domain.Ids;

namespace PetFamily.Specieses.Application.Abstractions;

public interface ISpeciesRepository
{
    Task UpdateAlreadyTrackingAsync(CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<Species> species, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<Species> species, CancellationToken cancellationToken = default);

    Task RemoveAsync(
        Species species,
        CancellationToken cancellationToken = default);

    Task<Species> GetSpeciesByIdWithBreedsAsync(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default);

    Task<Species> GetSpeciesByIdAsync(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default);

    Task<bool> SpeciesAndBreedExistsAsync(
        SpeciesId speciesId,
        BreedId breedId,
        CancellationToken cancellationToken = default);
}