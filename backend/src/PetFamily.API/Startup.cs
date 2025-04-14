using PetFamily.Infrastructure.DependencyInjection;

namespace PetFamily.API;

public static class Startup
{
    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLayers(configuration);
    }

    private static void AddLayers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgresSql(configuration);
    }
}