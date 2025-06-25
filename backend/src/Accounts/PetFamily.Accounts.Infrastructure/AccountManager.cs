using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetFemily.Accounts.Application;
using PetFemily.Accounts.Application.Dto;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class AccountManager : IAccountManager
{
    private readonly WriteAccountDbContext _writeDbContext;

    public AccountManager(
        WriteAccountDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<Guid> GetRoleIdByNameAsync(string roleName)
    {
        return await _writeDbContext.Roles.Where(x => x.Name == roleName).Select(x => x.Id).FirstAsync();
    }

    public async Task AddRefreshSession(RefreshSessions refreshSessions, CancellationToken cancellationToken)
    {
        await _writeDbContext.AddAsync(refreshSessions, cancellationToken);
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshSessions> GetRefreshSessionsAsync(
        Guid userId,
        Guid jti,
        CancellationToken cancellationToken)
    {
        var refreshSessions = await _writeDbContext.RefreshSessions
            .Where(x => x.UserId == userId && x.Jti == jti)
            .SingleAsync(cancellationToken);
        return refreshSessions;
    }

    public async Task DeleteRefreshSessionAsync(
        RefreshSessions refreshSessions,
        CancellationToken cancellationToken)
    {
        _writeDbContext.Remove(refreshSessions);
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}