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
    private readonly ReadAccountDbContext _readDbContext;

    public AccountManager(
        WriteAccountDbContext writeDbContext,
        ReadAccountDbContext readDbContext)
    {
        _readDbContext = readDbContext;
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

    public async Task<UserDto> GetFullInfoUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userDto = await _readDbContext.Users
            .Where(x => x.Id == userId)
            .Include(x => x.AdminAccount)
            .Include(x => x.VolunteerAccount)
            .Include(x => x.ParticipantAccount)
            .Include(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .SingleAsync(cancellationToken);

        return userDto;
    }

    public async Task<bool> UserExistByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userExist = await _readDbContext.Users
            .AnyAsync(x => x.Id == userId, cancellationToken);

        return userExist;
    }
}