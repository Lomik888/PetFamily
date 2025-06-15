using System.Threading.Channels;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Dtos;

namespace PetFamily.FileService.MessageQueue;

public class InvalidFilesMessageQueue : IChannelMessageQueue
{
    private readonly Channel<IEnumerable<FileToDeleteDto>> _channel =
        Channel.CreateUnbounded<IEnumerable<FileToDeleteDto>>();

    private readonly Channel<IEnumerable<FileToDeleteDto>> _channelCantDelete =
        Channel.CreateUnbounded<IEnumerable<FileToDeleteDto>>();

    public async Task AddPathsAsync(IEnumerable<FileToDeleteDto> path, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(path, cancellationToken);
    }

    // Демаю лучше сделать отдельное что-то, но я очень ленивый , будет тех долг
    public async Task AddPathsThanCantDeleteAsync(
        IEnumerable<FileToDeleteDto> path,
        CancellationToken cancellationToken = default)
    {
        await _channelCantDelete.Writer.WriteAsync(path, cancellationToken);
    }

    public async Task<IEnumerable<FileToDeleteDto>> ReadePathsAsync(CancellationToken cancellationToken = default)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }

    public IEnumerable<FileToDeleteDto> ReadeAllPaths()
    {
        var result = new List<FileToDeleteDto>();

        while (_channel.Reader.TryRead(out var paths))
        {
            result.AddRange(paths);
        }

        return result;
    }
}