using Microsoft.OpenApi.Models;
using PetFamily.API.Middleware;
using PetFamily.Core.Abstrations;
using PetFamily.FileService.DependencyInjection;
using PetFamily.Framework.Options;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Presentation.DependencyInjection;
using PetFamily.Volunteers.Presentation.DependencyInjection;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var psqlSection = builder.Configuration.GetRequiredSection("Psql_Connection_String");
var connectionString = psqlSection.GetValue<string>("Postgres_SQL") ??
                       throw new NullReferenceException("Postgres SQL Connection String missing");

builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
    new SqlConnectionFactory(connectionString));

builder.Services.AddVolunteersModule(builder.Configuration);
builder.Services.AddSpeciesModule(builder.Configuration);
builder.Services.AddFileService(builder.Configuration);

var seqConnectionString =
    builder.Configuration.GetRequiredSection(SeqOptions.SECTION_FOR_SEQ)
        .GetValue<string>(SeqOptions.CONNECTIONSTRING_FOR_SEQ);

var seqApiKey =
    builder.Configuration.GetRequiredSection(SeqOptions.SECTION_FOR_SEQ)
        .GetValue<string>(SeqOptions.API_KEY_FOR_SEQ);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Seq(
        restrictedToMinimumLevel: LogEventLevel.Information,
        serverUrl: seqConnectionString!,
        apiKey: seqApiKey)
    .WriteTo.Console(
        restrictedToMinimumLevel: LogEventLevel.Debug)
    .CreateLogger();

builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddControllers();

builder.Services.AddSerilog();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

namespace PetFamily.API
{
    public partial class Program;
}