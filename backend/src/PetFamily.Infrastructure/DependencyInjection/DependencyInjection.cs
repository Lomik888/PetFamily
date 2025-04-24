using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.BackgroundWorkers.HardDeleteWorker;
using PetFamily.Application.VolunteerUseCases;
using PetFamily.Infrastructure.DbContext.PostgresSQL;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlConnectionString =
            configuration
                .GetSection(ApplicationDbContextOptions.CONNECTIONSTRING_SECTION_FOR_POSTGRESSQL)
                .GetValue<string>(ApplicationDbContextOptions.CONNECTIONSTRING_FOR_POSTGRESSQL);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(psqlConnectionString));

        services.AddRepositories();
        services.AddOptions(configuration);
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
    }

    private static void AddBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<HardDeleteUnActiveEntitiesWorker>();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationDbContextOptions>(
            configuration.GetSection(ApplicationDbContextOptions.CONNECTIONSTRING_SECTION_FOR_POSTGRESSQL));
    }
}