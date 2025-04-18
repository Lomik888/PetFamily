using FluentValidation;
using PetFamily.API.Validations;
using PetFamily.Application.DependencyInjection;
using PetFamily.Infrastructure.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace PetFamily.API;

public static class Startup
{
    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddLayers(configuration);
        services.AddSwagger();
        services.AddValidation();
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

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

        services.AddFluentValidationAutoValidation(options =>
        {
            options.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
    }
}