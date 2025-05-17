namespace PetFamily.Infrastructure.Providers;

public interface IMinIoLimiter
{
    SemaphoreSlim SemaphoreSlim { get; }
}