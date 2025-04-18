using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteerUseCases.CreateVolunteer;

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
        services.AddScoped<ICreateVolunteerHandler, CreateVolunteerHandler>();
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}