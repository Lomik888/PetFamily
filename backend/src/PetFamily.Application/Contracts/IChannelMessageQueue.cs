namespace PetFamily.Application.Contracts;

public interface IChannelMessageQueue
{
    Task AddPathsAsync(IEnumerable<FileToDelete> path, CancellationToken cancellationToken = default);
    Task<IEnumerable<FileToDelete>> ReadePathsAsync(CancellationToken cancellationToken = default);

    Task AddPathsThanCantDeleteAsync(
        IEnumerable<FileToDelete> path,
        CancellationToken cancellationToken = default);

    IEnumerable<FileToDelete> ReadeAllPaths();
}