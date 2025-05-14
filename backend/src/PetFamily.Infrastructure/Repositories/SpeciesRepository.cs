using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application;
using PetFamily.Application.Repositories;
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