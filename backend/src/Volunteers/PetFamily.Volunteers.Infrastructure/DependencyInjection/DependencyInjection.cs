using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Enums;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Infrastructure.BackgroundWorkers;
using PetFamily.Volunteers.Infrastructure.DbContext;
using PetFamily.Volunteers.Infrastructure.Repositories;

namespace PetFamily.Volunteers.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void AddVolunteersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlSection = configuration.GetRequiredSection("Psql_Connection_String");
        var connectionString = psqlSection.GetValue<string>("Postgres_SQL") ??
                               throw new NullReferenceException("Postgres SQL Connection String missing");

        var hardDeleteWorkerSection =
            configuration.GetRequiredSection(HardDeleteUnActiveEntitiesWorkerOptions.OPTION_SECTION);

        services.AddScoped<IVolunteerRepository, VolunteerRepository>();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkTypes.Volunteers);

        services.AddHostedService<HardDeleteUnActiveEntitiesWorker>();

        services.Configure<HardDeleteUnActiveEntitiesWorkerOptions>(hardDeleteWorkerSection);

        services.AddScoped<IVolunteerRepository, VolunteerRepository>();

        services.AddDbContext<VolunteerDbContext>(x => x.UseNpgsql(connectionString));
    }
}