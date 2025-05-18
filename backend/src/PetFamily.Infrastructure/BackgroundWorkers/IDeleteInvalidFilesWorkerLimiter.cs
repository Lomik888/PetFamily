namespace PetFamily.Infrastructure.BackgroundWorkers;

public interface IDeleteInvalidFilesWorkerLimiter
{
    SemaphoreSlim SemaphoreSlim { get; }
}