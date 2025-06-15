using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core.Abstrations;
using PetFamily.FileService.BackgroundWorkers;
using PetFamily.FileService.MessageQueue;
using PetFamily.FileService.Providers;

namespace PetFamily.FileService.DependencyInjection;

public static class DependencyInjection
{
    public static void AddFileService(this IServiceCollection services, IConfiguration configuration)
    {
        var minIoSection = configuration.GetRequiredSection(MinIoProviderOptions.SECTION_NAME);
        var minIoEndpoint = minIoSection.GetValue<string>(MinIoProviderOptions.ENDPOINT_NAME);
        var minIoAccessKey = minIoSection.GetValue<string>(MinIoProviderOptions.ACCESSKEY_NAME);
        var minIoSecretKey = minIoSection.GetValue<string>(MinIoProviderOptions.SECRETKEY_NAME);
        var minIoSsl = minIoSection.GetValue<bool>(MinIoProviderOptions.WITH_SSL);

        services.AddSingleton<DeleteInvalidFilesWorkerLimiter>();
        services.AddHostedService<DeleteInvalidFilesWorker>();

        services.AddSingleton<IChannelMessageQueue, InvalidFilesMessageQueue>();

        services.Configure<MinIoProviderOptions>(minIoSection);
        services.AddScoped<IFilesProvider, MinIoProvider>();

        services.AddSingleton<MinIoLimiter>();
        services.AddMinio(options =>
        {
            options.WithEndpoint(minIoEndpoint);
            options.WithSSL(minIoSsl);
            options.WithCredentials(minIoAccessKey, minIoSecretKey);
            options.Build();
        });
    }
}