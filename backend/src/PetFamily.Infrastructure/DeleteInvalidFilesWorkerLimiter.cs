using PetFamily.Infrastructure.BackgroundWorkers;

namespace PetFamily.Infrastructure;

public class DeleteInvalidFilesWorkerLimiter : IDeleteInvalidFilesWorkerLimiter
{
    public SemaphoreSlim SemaphoreSlim { get; }

    public DeleteInvalidFilesWorkerLimiter()
    {
        SemaphoreSlim = new SemaphoreSlim(1, DeleteInvalidFilesWorker.SEMAPHORE_MAX_COUNT);
    }
}