using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Enums;
using PetFamily.Specieses.Application.Abstractions;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Infrastructure.Database;
using PetFamily.Specieses.Infrastructure.Repositories;
using PetFamily.Specieses.Marker;
using PetFamily.Specieses.Presentation.Contracts;

namespace PetFamily.Specieses.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static void AddSpeciesModule(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlSection = configuration.GetRequiredSection("Psql_Connection_String");
        var connectionString = psqlSection.GetValue<string>("Postgres_SQL") ??
                               throw new NullReferenceException("Postgres SQL Connection String missing");

        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        services.AddDbContext<SpeciesDbContext>(x => x.UseNpgsql(connectionString));

        services.AddScoped<ISpeciesExistenceContracts, SpeciesExistenceContracts>();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkTypes.Species);

        services.Scan(scan =>
            scan.FromAssemblies(typeof(SpeciesModule).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<,,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.Scan(scan =>
            scan.FromAssemblies(typeof(SpeciesModule).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandler<,,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(SpeciesModule).Assembly);
    }
}