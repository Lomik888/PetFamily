using Microsoft.Extensions.Options;
using PetFamily.FileService.Providers.MinIo;

namespace PetFamily.FileService.Providers;

public class MinIoLimiter : IMinIoLimiter
{
    public SemaphoreSlim SemaphoreSlim { get; }

    public MinIoLimiter(IOptions<MinIoProviderOptions> options)
    {
        SemaphoreSlim = new SemaphoreSlim(1, options.Value.RequestLimit);
    }
}