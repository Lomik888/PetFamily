using System.Data;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstrations;
using PetFamily.Specieses.Infrastructure.Database;

namespace PetFamily.Specieses.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly SpeciesDbContext _dbContext;

    public UnitOfWork(SpeciesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.CurrentTransaction == null)
            throw new Exception("Transaction has not been started");

        await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}