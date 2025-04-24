using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteerUseCases;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Infrastructure.DbContext.PostgresSQL;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public VolunteerRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsAlreadyTrackingAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Volunteer> GetByIdAsync(VolunteerId volunteerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Volunteers
            .SingleAsync(x => x.Id == volunteerId, cancellationToken);
    }

    public async Task<Volunteer> GetByIdAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Volunteers
            .SingleAsync(x => x.Id == volunteerId && x.IsActive == volunteerIsActive, cancellationToken);
    }

    public async Task<Volunteer> GetByIdWitchPetsAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Volunteers
            .Where(x => x.Id == volunteerId && x.IsActive == volunteerIsActive)
            .Include(x => x.Pets)
            .SingleAsync(cancellationToken);
    }

    public async Task HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
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
        {
            throw new ArgumentException(
                "Deleted at UTC time is not UTC or " +
                "the delay between the current and transmitted time is more than 5 minutes");
        }

        var query = await _dbContext.Volunteers
            .Where(x =>
                x.IsActive == isActive )//&&
              //  x.DeletedAt != null &&
             //   x.DeletedAt!.Value <= deletedAtUtcNow)
            .ExecuteDeleteAsync(cancellationToken);

        return query;
    }
}