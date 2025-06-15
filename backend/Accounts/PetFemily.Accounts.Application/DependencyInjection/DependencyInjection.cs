using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;

namespace PetFemily.Accounts.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddAccountsApplicationLayer(this IServiceCollection services)
    {
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