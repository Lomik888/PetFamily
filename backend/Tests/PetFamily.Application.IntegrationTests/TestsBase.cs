using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations;
using PetFamily.Data.Tests;
using PetFamily.Specieses.Infrastructure.Database;
using PetFamily.Volunteers.Infrastructure.DbContext;

namespace PetFamily.Application.IntegrationTests;

public class TestsBase : IClassFixture<IntegrationsTestsWebAppFactory>, IAsyncLifetime
{
    protected readonly IntegrationsTestsWebAppFactory Factory;
    protected readonly ISqlConnectionFactory SqlConnectionFactory;
    protected readonly IServiceScope Scope;
    protected readonly VolunteerDbContext VolunteerDbContext;
    protected readonly SpeciesDbContext SpeciesDbContext;
    protected readonly TestDbContext TestDbContext;
    protected readonly IFixture Fixture;
    protected readonly Random Random = new Random();

    private readonly Func<Task> _resetDatabase;

    protected TestsBase(IntegrationsTestsWebAppFactory factory)
    {
        Factory = factory;
        _resetDatabase = factory.ResetDatabaseAsync;
        Scope = factory.Services.CreateScope();
        VolunteerDbContext = Scope.ServiceProvider.GetRequiredService<VolunteerDbContext>();
        SpeciesDbContext = Scope.ServiceProvider.GetRequiredService<SpeciesDbContext>();
        TestDbContext = Scope.ServiceProvider.GetRequiredService<TestDbContext>();
        Fixture = new Fixture();
        SqlConnectionFactory = Scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await _resetDatabase();
    }
}