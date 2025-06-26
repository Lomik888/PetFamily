using Microsoft.EntityFrameworkCore;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application;

public interface IReadDbContext
{
    IQueryable<User> Users { get; }
    IQueryable<Role> Roles { get; }
    IQueryable<Permission> Permissions { get; }
    IQueryable<AdminAccount> AdminAccount { get; }
    IQueryable<VolunteerAccount> VolunteerAccount { get; }
    IQueryable<ParticipantAccount> ParticipantAccount { get; }
    IQueryable<RefreshSessions> RefreshSessions { get; }
}