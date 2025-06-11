using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Validation;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Infrastructure.DbContext.PostgresSQL;

namespace PetFamily.Volunteers.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly VolunteerDbContext _dbContext;
    private readonly ILogger<VolunteerRepository> _logger;

    public VolunteerRepository(VolunteerDbContext dbContext, ILogger<VolunteerRepository> logger)
    {
        _dbContext = dbContext ??
                     throw new ArgumentNullException(nameof(dbContext), "dbContext cannot be null");
        _logger = logger ??
                  throw new ArgumentNullException(nameof(logger), "logger cannot be null");
    }

    public async Task AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(volunteer);

        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer added: {volunteer}", volunteer);
    }

    public async Task UpdateAsAlreadyTrackingAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(volunteer);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer updated: {volunteer}", volunteer);
    }

    public async Task<Volunteer> GetByIdAsync(VolunteerId volunteerId, CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(volunteerId);
        Validator.Guard.NotEmpty(volunteerId.Value);

        var volunteer = await _dbContext.Volunteers
            .SingleAsync(x => x.Id == volunteerId, cancellationToken);

        _logger.LogInformation("Volunteer returned by id: {volunteer}", volunteer);

        return volunteer;
    }

    public async Task<Volunteer> GetByIdAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(volunteerId);
        Validator.Guard.NotEmpty(volunteerId.Value);

        var volunteer = await _dbContext.Volunteers
            .SingleAsync(x =>
                    x.Id == volunteerId &&
                    x.IsActive == volunteerIsActive,
                cancellationToken);

        _logger.LogInformation("Volunteer returned by id: {volunteer}", volunteer);

        return volunteer;
    }

    public async Task<Volunteer> GetByIdWithPetsAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(volunteerId);
        Validator.Guard.NotEmpty(volunteerId.Value);

        var volunteer = await _dbContext.Volunteers
            .Where(x =>
                x.Id == volunteerId &&
                x.IsActive == volunteerIsActive)
            .Include(x => x.Pets)
            .SingleAsync(cancellationToken);

        _logger.LogInformation("Volunteer returned by id: {volunteer}", volunteer);

        return volunteer;
    }

    public async Task<Volunteer> GetByIdWithPetsAsync(VolunteerId volunteerId, CancellationToken cancellationToken)
    {
        Validator.Guard.NotNull(volunteerId);
        Validator.Guard.NotEmpty(volunteerId.Value);

        var volunteer = await _dbContext.Volunteers
            .Where(x =>
                x.Id == volunteerId)
            .Include(x => x.Pets)
            .SingleAsync(cancellationToken);

        _logger.LogInformation("Volunteer returned by id: {volunteer}", volunteer);

        return volunteer;
    }

    public async Task HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        Validator.Guard.NotNull(volunteer);

        _dbContext.Volunteers.Remove(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer hard deleted: {volunteer}", volunteer);
    }

    public async Task<int> HardDeleteAllSofDeletedAsync(
        DateTime deletedAtUtcNow,
        int delay,
        CancellationToken cancellationToken = default)
    {
        Validator.Guard.IsPastRetentionPeriod(deletedAtUtcNow, delay);

        var query = _dbContext.Volunteers
            .Where(x =>
                x.IsActive == false &&
                x.DeletedAt != null &&
                x.DeletedAt.Value <= deletedAtUtcNow);

        var count = await query.ExecuteDeleteAsync(cancellationToken);

        _logger.LogInformation("Hard-deleted {count} soft-deleted volunteers", count);

        return count;
    }
}