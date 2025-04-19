using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteerUseCases;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    private const string CONNECTION_STRING_KEY_FOR_POSTGRESSQL = "Postgres_SQL";

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlConnectionString =
            configuration.GetConnectionString(CONNECTION_STRING_KEY_FOR_POSTGRESSQL) ??
            throw new InvalidOperationException("connection string is missing");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(psqlConnectionString));

        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
    }
}