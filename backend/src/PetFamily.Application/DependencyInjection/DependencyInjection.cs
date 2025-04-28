using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.BackgroundWorkers.HardDeleteWorker;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Activate;
using PetFamily.Application.VolunteerUseCases.Create;
using PetFamily.Application.VolunteerUseCases.Delete;
using PetFamily.Application.VolunteerUseCases.UpdateDetailsForHelps;
using PetFamily.Application.VolunteerUseCases.UpdateMainInfo;
using PetFamily.Application.VolunteerUseCases.UpdateSocialNetworks;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();
        services.AddValidation();
        services.AddBackgroundServices();
        services.AddOptions(configuration);
    }

    private static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<Guid, ErrorList, CreateVolunteerCommand>,
                CreateVolunteerHandler>();

        services
            .AddScoped<ICommandHandler<Guid, ErrorList, UpdateMainInfoVolunteerCommand>,
                UpdateMainInfoVolunteerHandler>();

        services
            .AddScoped<ICommandHandler<ErrorList, UpdateVolunteersSocialNetworksCommand>,
                UpdateVolunteersSocialNetworksHandler>();

        services
            .AddScoped<ICommandHandler<ErrorList, UpdateVolunteersDetailsForHelpCommand>,
                UpdateVolunteersDetailsForHelpHandler>();

        services
            .AddScoped<ICommandHandler<ErrorList, DeleteVolunteerCommand>,
                DeleteVolunteerHandler>();
        services
            .AddScoped<ICommandHandler<ErrorList, ActivateVolunteerCommand>,
                ActivateVolunteerHandle>();
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    private static void AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<HardDeleteUnActiveEntitiesWorker>();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HardDeleteUnActiveEntitiesWorkerOptions>(
            configuration.GetRequiredSection(HardDeleteUnActiveEntitiesWorkerOptions.OPTIONSECTION));
    }
}