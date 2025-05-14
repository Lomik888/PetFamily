using System.Data;

namespace PetFamily.Application;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

    void Dispose();

    ValueTask DisposeAsync();
}