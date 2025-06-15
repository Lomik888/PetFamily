using PetFamily.API;
using PetFamily.API.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

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