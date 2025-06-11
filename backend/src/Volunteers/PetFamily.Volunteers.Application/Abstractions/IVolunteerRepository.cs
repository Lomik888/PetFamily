using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Abstractions;

public interface IVolunteerRepository
{
    public Task AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task UpdateAsAlreadyTrackingAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Volunteer> GetByIdAsync(VolunteerId volunteerId, CancellationToken cancellationToken = default);
    public Task HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default);

    public Task<Volunteer> GetByIdAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken = default);

    public Task<Volunteer> GetByIdWithPetsAsync(
        VolunteerId volunteerId,
        bool volunteerIsActive,
        CancellationToken cancellationToken);

    public Task<Volunteer> GetByIdWithPetsAsync(
        VolunteerId volunteerId,
        CancellationToken cancellationToken);

    Task<int> HardDeleteAllSofDeletedAsync(
        DateTime deletedAtUtcNow,
        int delay,
        CancellationToken cancellationToken = default);
}