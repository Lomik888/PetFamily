using Microsoft.Extensions.Options;

namespace PetFamily.FileService.Providers;

public class MinIoLimiter
{
    public SemaphoreSlim SemaphoreSlim { get; }

    public MinIoLimiter(IOptions<MinIoProviderOptions> options)
    {
        SemaphoreSlim = new SemaphoreSlim(1, options.Value.RequestLimit);
    }
}