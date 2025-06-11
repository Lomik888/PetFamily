namespace PetFamily.FileService.BackgroundWorkers;

public class DeleteInvalidFilesWorkerLimiter
{
    public SemaphoreSlim SemaphoreSlim { get; }

    public DeleteInvalidFilesWorkerLimiter()
    {
        SemaphoreSlim = new SemaphoreSlim(1, DeleteInvalidFilesWorker.SEMAPHORE_MAX_COUNT);
    }
}