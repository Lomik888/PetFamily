using PetFamily.Core.Dtos;

namespace PetFamily.Core.Abstrations;

public interface IChannelMessageQueue
{
    Task AddPathsAsync(IEnumerable<FileToDeleteDto> path, CancellationToken cancellationToken = default);
    Task<IEnumerable<FileToDeleteDto>> ReadePathsAsync(CancellationToken cancellationToken = default);

    Task AddPathsThanCantDeleteAsync(
        IEnumerable<FileToDeleteDto> path,
        CancellationToken cancellationToken = default);

    IEnumerable<FileToDeleteDto> ReadeAllPaths();
}