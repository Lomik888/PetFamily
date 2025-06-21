using DotNetEnv;
using PetFamily.API;
using PetFamily.API.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Host.UseSerilog();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var seedRolesPermissions = scope.ServiceProvider.GetRequiredService<SeedRolesPermissions>();
await seedRolesPermissions.Seed();
var seedAdmin = scope.ServiceProvider.GetRequiredService<SeedAccounts>();
await seedAdmin.Seed();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace PetFamily.API
{
    public partial class Program;
}