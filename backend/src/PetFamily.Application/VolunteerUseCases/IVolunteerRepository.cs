using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;

namespace PetFamily.Application.VolunteerUseCases;

public interface IVolunteerRepository
{
    public Task AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task UpdateAsAlreadyTrackingAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Volunteer> GetByIdAsync(VolunteerId volunteerId, CancellationToken cancellationToken = default);
}