namespace PetFamily.FileService.Providers;

public interface IMinIoLimiter
{
    SemaphoreSlim SemaphoreSlim { get; }
}