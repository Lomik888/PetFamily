using PetFamily.Application.DependencyInjection;
using PetFamily.Infrastructure.DependencyInjection;

namespace PetFamily.API;

public static class Startup
{
    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddLayers(configuration);
        services.AddSwagger();
    }

    private static void AddLayers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplicationLayer();
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
    }
}