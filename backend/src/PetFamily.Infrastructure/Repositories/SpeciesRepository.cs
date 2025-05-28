using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application;
using PetFamily.Application.Repositories;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.Ids;
using PetFamily.Infrastructure.DbContext.PostgresSQL;
using PetFamily.Shared.Validation;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<SpeciesRepository> _logger;

    public SpeciesRepository(ApplicationDbContext dbContext, ILogger<SpeciesRepository> logger)
    {
        _dbContext = dbContext ??
                     throw new ArgumentNullException(nameof(dbContext), "DbContext is missing");
        _logger = logger ??
                  throw new ArgumentNullException(nameof(logger), "Logger is missing");
    }

    public async Task UpdateAlreadyTrackingAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Species> species, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddRangeAsync(species, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<Species> species, CancellationToken cancellationToken = default)
    {
        _dbContext.UpdateRange(species);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Species> GetSpeciesByIdAsync(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(speciesId);
        return await _dbContext.Species
            .Where(x => x.Id == speciesId)
            .SingleAsync(cancellationToken);
    }

    public async Task<Species> GetSpeciesByIdWithBreedsAsync(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(speciesId);
        return await _dbContext.Species
            .Where(x => x.Id == speciesId)
            .Include(x => x.Breeds)
            .SingleAsync(cancellationToken);
    }

    public async Task RemoveAsync(
        Species species,
        CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(species);
        _dbContext.Species.Remove(species);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SpeciesAndBreedExistsAsync(
        SpeciesId speciesId,
        BreedId breedId,
        CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(speciesId);
        Validator.Guard.NotNull(breedId);
        Validator.Guard.NotEmpty(speciesId.Value);
        Validator.Guard.NotEmpty(breedId.Value);

        var result = await _dbContext.Species
            .AnyAsync(
                x => x.Id == speciesId &&
                     x.Breeds.Any(b => b.Id == breedId), cancellationToken);

        _logger.LogInformation(
            "Species {speciesId.Value} and Breed {breedId.Value} exist result:{result}",
            speciesId.Value,
            breedId.Value,
            result);

        return result;
    }
}