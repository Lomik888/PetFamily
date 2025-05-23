﻿using Microsoft.Extensions.Options;
using PetFamily.Infrastructure.Providers.MinIo;

namespace PetFamily.Infrastructure.Providers;

public class MinIoLimiter : IMinIoLimiter
{
    public SemaphoreSlim SemaphoreSlim { get; }

    public MinIoLimiter(IOptions<MinIoProviderOptions> options)
    {
        SemaphoreSlim = new SemaphoreSlim(1, options.Value.RequestLimit);
    }
}