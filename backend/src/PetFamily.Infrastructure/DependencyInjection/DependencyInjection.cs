using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application;
using PetFamily.Application.Contracts;
using PetFamily.Application.Providers;
using PetFamily.Application.Repositories;
using PetFamily.Infrastructure.BackgroundWorkers;
using PetFamily.Infrastructure.DbContext.PostgresSQL;
using PetFamily.Infrastructure.MessageQueue;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Providers.MinIo;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlConnectionString =
            configuration
                .GetRequiredSection(ApplicationDbContextOptions.CONNECTIONSTRING_SECTION_FOR_POSTGRESSQL)
                .GetValue<string>(ApplicationDbContextOptions.CONNECTIONSTRING_FOR_POSTGRESSQL);

        services.AddRepositories()
            .AddMinIo(configuration)
            .AddUnitOfWork()
            .AddOptionsPattern(configuration)
            .AddLimiters()
            .AddChannels()
            .AddDapperFactory()
            .AddBackgroundService()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(psqlConnectionString));
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        return services.AddScoped<ISpeciesRepository, SpeciesRepository>();
    }

    private static IServiceCollection AddDapperFactory(this IServiceCollection services)
    {
        return services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
    }

    private static IServiceCollection AddLimiters(this IServiceCollection services)
    {
        services.AddSingleton<IDeleteInvalidFilesWorkerLimiter, DeleteInvalidFilesWorkerLimiter>();
        return services.AddSingleton<IMinIoLimiter, MinIoLimiter>();
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        return services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static IServiceCollection AddChannels(this IServiceCollection services)
    {
        return services.AddSingleton<IChannelMessageQueue, InvalidFilesMessageQueue>();
    }

    private static IServiceCollection AddBackgroundService(this IServiceCollection services)
    {
        return services.AddHostedService<DeleteInvalidFilesWorker>();
    }

    private static IServiceCollection AddOptionsPattern(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MinIoProviderOptions>(configuration.GetRequiredSection("MinIO"));
    }

    private static IServiceCollection AddMinIo(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFilesProvider, MinIoProvider>();

        var minIoSection = configuration.GetRequiredSection(MinIoProviderOptions.SECTION_NAME);
        var minIoEndpoint = minIoSection.GetValue<string>(MinIoProviderOptions.ENDPOINT_NAME);
        var minIoAccessKey = minIoSection.GetValue<string>(MinIoProviderOptions.ACCESSKEY_NAME);
        var minIoSecretKey = minIoSection.GetValue<string>(MinIoProviderOptions.SECRETKEY_NAME);
        var minIoSsl = minIoSection.GetValue<bool>(MinIoProviderOptions.WITH_SSL);

        return services.AddMinio(options =>
        {
            options.WithEndpoint(minIoEndpoint);
            options.WithSSL(minIoSsl);
            options.WithCredentials(minIoAccessKey, minIoSecretKey);
            options.Build();
        });
    }
}