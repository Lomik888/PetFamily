﻿using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.BackgroundWorkers.HardDeleteWorker;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Queries.GetPet;
using PetFamily.Domain.VolunteerContext.SharedVO;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommands()
            .AddValidation()
            .AddBackgroundServices()
            .AddOptions(configuration);
        
        SqlMapper.AddTypeHandler(new JsonTypeHandler<IEnumerable<DetailsForHelp>>());
        SqlMapper.AddTypeHandler(new JsonTypeHandler<IEnumerable<File>>());
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(IQueryHandler<,,>), typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

        return services.Scan(scan =>
            scan.FromAssemblies(typeof(DependencyInjection).Assembly)
                .AddClasses(classes =>
                    classes.AssignableToAny(typeof(ICommandHandler<,,>), typeof(ICommandHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
    }

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        return services.AddHostedService<HardDeleteUnActiveEntitiesWorker>();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<HardDeleteUnActiveEntitiesWorkerOptions>(
            configuration.GetRequiredSection(HardDeleteUnActiveEntitiesWorkerOptions.OPTIONSECTION));
    }
}