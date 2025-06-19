using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.DapperModifications;
using PetFamily.SharedKernel.ValueObjects;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Volunteers.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddVolunteersApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        SqlMapper.AddTypeHandler(new JsonTypeHandler<IEnumerable<DetailsForHelp>>());
        SqlMapper.AddTypeHandler(new JsonTypeHandler<IEnumerable<File>>());

        services.Scan(scan =>
            scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(IQueryHandler<,,>), typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

        services.Scan(scan =>
            scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(ICommandHandler<,,>), typeof(ICommandHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}