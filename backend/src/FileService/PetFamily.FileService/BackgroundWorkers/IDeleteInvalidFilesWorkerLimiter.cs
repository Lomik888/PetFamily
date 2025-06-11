namespace PetFamily.FileService.BackgroundWorkers;

public interface IDeleteInvalidFilesWorkerLimiter
{
    SemaphoreSlim SemaphoreSlim { get; }
}