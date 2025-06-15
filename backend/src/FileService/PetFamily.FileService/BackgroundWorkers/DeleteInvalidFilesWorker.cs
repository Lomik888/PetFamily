using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Dtos;
using Exception = System.Exception;

namespace PetFamily.FileService.BackgroundWorkers;

public class DeleteInvalidFilesWorker : BackgroundService
{
    private const int TIMER_INTERVAL_MIN = 30;
    public const int SEMAPHORE_MAX_COUNT = 10;
    private const int MAX_COUNT_RETRY = 5;
    private readonly ILogger<DeleteInvalidFilesWorker> _logger;
    private readonly IServiceScopeFactory _scope;
    private readonly IChannelMessageQueue _channel;
    private readonly DeleteInvalidFilesWorkerLimiter _semaphore;

    public DeleteInvalidFilesWorker(
        ILogger<DeleteInvalidFilesWorker> logger,
        IServiceScopeFactory scope,
        IChannelMessageQueue channel,
        DeleteInvalidFilesWorkerLimiter semaphore)
    {
        _logger = logger;
        _scope = scope;
        _channel = channel;
        _semaphore = semaphore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(TIMER_INTERVAL_MIN));

        do
        {
            _logger.LogInformation("Deleting invalid files start");
            await PerformDeleteInvalidFilesAsync(stoppingToken);
            _logger.LogInformation("Deleting invalid files end");
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task PerformDeleteInvalidFilesAsync(CancellationToken cancellationToken)
    {
        var filesPaths = _channel.ReadeAllPaths();
        var minIoProvider = _scope.CreateAsyncScope().ServiceProvider.GetRequiredService<IFilesProvider>();

        var deleteInvalidFilesTask = filesPaths
            .Select<FileToDeleteDto, Task<(FileToDeleteDto File, bool IsSuccess)>>(async x =>
            {
                try
                {
                    await _semaphore.SemaphoreSlim.WaitAsync(cancellationToken);
                    var result = await minIoProvider.RemoveAsync(x.Path, cancellationToken);
                    if (result.IsSuccess == false)
                    {
                        return (x, false);
                    }

                    return (x, true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete invalid files{x.Path}", x.Path);
                    return (x, false);
                }
                finally
                {
                    _semaphore.SemaphoreSlim.Release();
                }
            }).ToList();

        var results = await Task.WhenAll(deleteInvalidFilesTask);

        var deletedFiles = results
            .Where(x => x.IsSuccess == true).Select(x => x.File)
            .ToList();
        var deletedFailedFiles = results
            .Where(x => x.IsSuccess == false && x.File.CountRetry > MAX_COUNT_RETRY)
            .Select(x => x.File)
            .ToList();
        var filesThanCantDelete = deletedFailedFiles
            .Where(x => x.CountRetry > MAX_COUNT_RETRY)
            .ToList();

        if (deletedFiles.Count == 0)
        {
            _logger.LogInformation("DeleteInvalidFilesWorker deleted files{deletedFiles.Count}", deletedFiles.Count);
        }

        if (deletedFailedFiles.Count > 0)
        {
            _logger.LogInformation(
                "DeleteInvalidFilesWorker deleted failed files{deletedFailedFiles.Count}",
                deletedFailedFiles.Count);

            foreach (var file in deletedFailedFiles)
            {
                file.PlusCountRetry();
            }

            await _channel.AddPathsThanCantDeleteAsync(filesThanCantDelete, cancellationToken);
            await _channel.AddPathsAsync(deletedFailedFiles, cancellationToken);
        }
    }
}