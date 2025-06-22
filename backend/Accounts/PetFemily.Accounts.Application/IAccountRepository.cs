using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application;

public interface IAccountManager
{
    Task<Guid> GetRoleIdByNameAsync(string roleName);
    Task AddRefreshSession(RefreshSessions refreshSessions, CancellationToken cancellationToken);

    Task<RefreshSessions> GetRefreshSessionsAsync(
        Guid userId,
        Guid jti,
        CancellationToken cancellationToken);
    Task DeleteRefreshSessionAsync(
        RefreshSessions refreshSessions,
        CancellationToken cancellationToken);
}