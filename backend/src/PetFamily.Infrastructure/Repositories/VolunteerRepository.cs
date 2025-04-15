using Microsoft.EntityFrameworkCore;
using PetFamily.Application.VolunteerUseCases;
using PetFamily.Domain.VolunteerContext.Entities;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly ApplicationDbContext _context;

    public VolunteerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(volunteer, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}