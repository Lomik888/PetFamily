using Microsoft.EntityFrameworkCore;
using PetFemily.Accounts.Application;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class AccountManager : IAccountManager
{
    private readonly AccountDbContext _dbContext;

    public AccountManager(AccountDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> GetRoleIdByNameAsync(string roleName)
    {
        return await _dbContext.Roles.Where(x => x.Name == roleName).Select(x => x.Id).FirstAsync();
    }

    public async Task AddRefreshSession(RefreshSessions refreshSessions, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(refreshSessions, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshSessions> GetRefreshSessionsAsync(
        Guid userId,
        Guid jti,
        CancellationToken cancellationToken)
    {
        var refreshSessions = await _dbContext.RefreshSessions
            .Where(x => x.UserId == userId && x.Jti == jti)
            .SingleAsync(cancellationToken);
        return refreshSessions;
    }

    public async Task DeleteRefreshSessionAsync(
        RefreshSessions refreshSessions,
        CancellationToken cancellationToken)
    {
        _dbContext.Remove(refreshSessions);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}