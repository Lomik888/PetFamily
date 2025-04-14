using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.VolunteerUseCases.CreateVolunteer;

namespace PetFamily.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateVolunteerHandler, CreateVolunteerHandler>();
    }
}