using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Application.DependencyInjection;
using PetFamily.Volunteers.Infrastructure.DependencyInjection;

namespace PetFamily.Volunteers.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static void AddVolunteersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddVolunteersInfrastructure(configuration);
        services.AddVolunteersApplicationLayer(configuration);
    }
}