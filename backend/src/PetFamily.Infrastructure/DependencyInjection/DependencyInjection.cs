using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.BackgroundWorkers;
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
                .GetConnectionString(ApplicationDbContextOptions.CONNECTIONSTRING_SECTION_FOR_POSTGRESSQL) ??
            throw new InvalidOperationException("connection string is missing");

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
        services.AddOptions<ApplicationDbContextOptions>()
            .Bind(configuration.GetRequiredSection("ConnectionStrings"))
            .Validate(x =>
                    x.ConnectionString is not null,
                "connection string is missing")
            .ValidateOnStart();
    }
}