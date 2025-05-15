namespace PetFamily.Infrastructure;

public interface IMinIoLimiter
{
    SemaphoreSlim SemaphoreSlim { get; }
}