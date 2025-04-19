using FluentValidation;
using PetFamily.API.Validations;
using PetFamily.Application.DependencyInjection;
using PetFamily.Infrastructure.DependencyInjection;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace PetFamily.API;

public static class Startup
{
    private const string CONNECTION_STRING_KEY_FOR_SEQ = "Seq";
    private const string API_KEY_FOR_SEQ = "Seq_Api_Key";

    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddLayers(configuration);
        services.AddSwagger();
        services.AddValidation();
        services.AddLogging(configuration);
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

    private static void AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var seqConnectionString =
            configuration.GetConnectionString(CONNECTION_STRING_KEY_FOR_SEQ) ??
            throw new InvalidOperationException("connection string is missing");

        var seqApiKey =
            configuration.GetSection("Keys").GetValue<string>(API_KEY_FOR_SEQ) ??
            throw new InvalidOperationException("Key is missing");

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Seq(
                restrictedToMinimumLevel: LogEventLevel.Information,
                serverUrl: seqConnectionString,
                apiKey: seqApiKey)
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();

        services.AddSerilog();
    }
}