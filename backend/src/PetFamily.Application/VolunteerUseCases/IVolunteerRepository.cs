namespace PetFamily.Application.VolunteerUseCases;

public interface IVolunteerRepository
{
    public Task AddAsync(Domain.VolunteerContext.Entities.Volunteer volunteer, CancellationToken cancellationToken = default);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}