using PetFamily.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguration(builder.Configuration);

var app = builder.Build();

app.MapSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();