using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.VolunteerUseCases;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Infrastructure.DbContext.PostgresSQL;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<VolunteerRepository> _logger;

    public VolunteerRepository(ApplicationDbContext dbContext, ILogger<VolunteerRepository> logger)
    {
        _dbContext = dbContext ??
                     throw new ArgumentNullException(nameof(dbContext), "dbContext cannot be null");
        _logger = logger;
    }

    public async Task AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        if (volunteer is null)
            throw new ArgumentNullException(nameof(volunteer), "volunteer cannot be null");

        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsAlreadyTrackingAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        if (volunteer is null)
            throw new ArgumentNullException(nameof(volunteer), "volunteer cannot be null");

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Volunteer> GetByIdAsync(VolunteerId volunteerId, CancellationToken cancellationToken = default)
    {
        if (volunteerId.Value != Guid.Empty)
            throw new ArgumentException("VolunteerId cannot be null", nameof(VolunteerId));

        return await _dbContext.Volunteers
            .SingleAsync(x => x.Id == volunteerId, cancellationToken);
    }

    public async Task<Volunteer> GetByIdAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken = default)
    {
        if (volunteerId.Value != Guid.Empty)
            throw new ArgumentException("VolunteerId cannot be null", nameof(VolunteerId));

        return await _dbContext.Volunteers
            .SingleAsync(x => x.Id == volunteerId && x.IsActive == volunteerIsActive, cancellationToken);
    }

    public async Task<Volunteer> GetByIdWithPetsAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken = default)
    {
        if (volunteerId.Value != Guid.Empty)
            throw new ArgumentException("VolunteerId cannot be null", nameof(VolunteerId));

        return await _dbContext.Volunteers
            .Where(x => x.Id == volunteerId && x.IsActive == volunteerIsActive)
            .Include(x => x.Pets)
            .SingleAsync(cancellationToken);
    }

    public async Task HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        if (volunteer is null)
            throw new ArgumentNullException(nameof(volunteer), "volunteer cannot be null");

        _dbContext.Volunteers.Remove(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> HardDeleteAllSofDeletedAsync(
        bool isActive,
        DateTime deletedAtUtcNow,
        CancellationToken cancellationToken = default)
    {
        var dateTimeUtcNow = DateTime.UtcNow;
        var lastValidDateTime = dateTimeUtcNow.AddMinutes(-5);

        if (deletedAtUtcNow.Kind != DateTimeKind.Utc && deletedAtUtcNow <= lastValidDateTime)
            throw new ArgumentException(
                "Deleted at UTC time is not UTC or " +
                "the delay between the current and transmitted time is more than 5 minutes");

        var query = _dbContext.Volunteers
            .Where(x =>
                x.IsActive == isActive &&
                x.DeletedAt != null &&
                x.DeletedAt.Value <= deletedAtUtcNow);

        var count = await query.ExecuteDeleteAsync(cancellationToken);

        return count;
    }
}