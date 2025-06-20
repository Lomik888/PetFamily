using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Presentation.DependencyInjection;
using PetFamily.Core.Abstrations;
using PetFamily.FileService.DependencyInjection;
using PetFamily.Framework.Options;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Presentation.DependencyInjection;
using PetFamily.Volunteers.Presentation.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace PetFamily.API;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlSection = configuration.GetRequiredSection("Psql_Connection_String");

        var connectionString = psqlSection.GetValue<string>("Postgres_SQL") ??
                               throw new NullReferenceException("Postgres SQL Connection String missing");

        var seqConnectionString = configuration.GetRequiredSection(SeqOptions.SECTION_FOR_SEQ)
            .GetValue<string>(SeqOptions.CONNECTIONSTRING_FOR_SEQ);

        var seqApiKey = configuration.GetRequiredSection(SeqOptions.SECTION_FOR_SEQ)
            .GetValue<string>(SeqOptions.API_KEY_FOR_SEQ);

        services.AddScoped<SeedRolesPermissions>();
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        services.AddVolunteersModule(configuration);
        services.AddAccountsModule(configuration);
        services.AddSpeciesModule(configuration);
        services.AddFileService(configuration);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Seq(
                restrictedToMinimumLevel: LogEventLevel.Information,
                serverUrl: seqConnectionString!,
                apiKey: seqApiKey)
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.AddControllers();
        services.AddSerilog();
    }
}