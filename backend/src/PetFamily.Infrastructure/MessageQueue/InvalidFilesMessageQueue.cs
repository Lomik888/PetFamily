using System.Threading.Channels;
using PetFamily.Application.Contracts;

namespace PetFamily.Infrastructure.MessageQueue;

public class InvalidFilesMessageQueue : IChannelMessageQueue
{
    private readonly Channel<IEnumerable<FileToDelete>> _channel = Channel.CreateUnbounded<IEnumerable<FileToDelete>>();

    private readonly Channel<IEnumerable<FileToDelete>> _channelCantDelete =
        Channel.CreateUnbounded<IEnumerable<FileToDelete>>();

    public async Task AddPathsAsync(IEnumerable<FileToDelete> path, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(path, cancellationToken);
    }

    // Демаю лучше сделать отдельное что-то, но я очень ленивый , будет тех долг
    public async Task AddPathsThanCantDeleteAsync(
        IEnumerable<FileToDelete> path,
        CancellationToken cancellationToken = default)
    {
        await _channelCantDelete.Writer.WriteAsync(path, cancellationToken);
    }

    public async Task<IEnumerable<FileToDelete>> ReadePathsAsync(CancellationToken cancellationToken = default)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }

    public IEnumerable<FileToDelete> ReadeAllPaths()
    {
        var result = new List<FileToDelete>();

        while (_channel.Reader.TryRead(out var paths))
        {
            result.AddRange(paths);
        }

        return result;
    }
}