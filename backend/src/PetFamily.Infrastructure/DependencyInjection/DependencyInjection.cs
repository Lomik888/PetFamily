using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application;
using PetFamily.Application.BackgroundWorkers.HardDeleteWorker;
using PetFamily.Application.Providers;
using PetFamily.Application.VolunteerUseCases;
using PetFamily.Infrastructure.DbContext.PostgresSQL;
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

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(psqlConnectionString));

        services.AddRepositories();
        services.AddMinIo(configuration);
        services.AddProviders();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
    }

    private static void AddBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<HardDeleteUnActiveEntitiesWorker>();
    }

    private static void AddMinIo(this IServiceCollection services, IConfiguration configuration)
    {
        var minIoSection = configuration.GetRequiredSection(MinIoProviderOptions.SECTION_NAME);
        var minIoEndpoint = minIoSection.GetValue<string>(MinIoProviderOptions.ENDPOINT_NAME);
        var minIoAccessKey = minIoSection.GetValue<string>(MinIoProviderOptions.ACCESSKEY_NAME);
        var minIoSecretKey = minIoSection.GetValue<string>(MinIoProviderOptions.SECRETKEY_NAME);
        var minIoSsl = minIoSection.GetValue<bool>(MinIoProviderOptions.WITH_SSL);

        services.AddMinio(options =>
        {
            options.WithEndpoint(minIoEndpoint);
            options.WithSSL(minIoSsl);
            options.WithCredentials(minIoAccessKey, minIoSecretKey);
            options.Build();
        });
    }

    private static void AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IFilesProvider, MinIoProvider>();
    }
}