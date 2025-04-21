using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Create;
using PetFamily.Application.VolunteerUseCases.UpdateDetailsForHelps;
using PetFamily.Application.VolunteerUseCases.UpdateMainInfo;
using PetFamily.Application.VolunteerUseCases.UpdateSocialNetworks;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddServices();
        services.AddValidation();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<Guid, ErrorCollection, CreateVolunteerCommand>,
                CreateVolunteerHandler>();

        services
            .AddScoped<ICommandHandler<Guid, ErrorCollection, UpdateMainInfoVolunteerCommand>,
                UpdateMainInfoVolunteerHandler>();

        services
            .AddScoped<ICommandHandler<ErrorCollection, UpdateVolunteersSocialNetworksCommand>,
                UpdateVolunteersSocialNetworksHandler>();

        services
            .AddScoped<ICommandHandler<ErrorCollection, UpdateVolunteersDetailsForHelpCommand>,
                UpdateVolunteersDetailsForHelpHandler>();
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}