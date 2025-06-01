using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Infrastructure.DbContext.PostgresSQL;

namespace PetFamily.Application.IntegrationTests;

public class TestsBase : IClassFixture<IntegrationsTestsWebAppFactory>, IAsyncLifetime
{
    protected readonly IntegrationsTestsWebAppFactory Factory;
    protected readonly ISqlConnectionFactory SqlConnectionFactory;
    protected readonly IServiceScope Scope;
    protected readonly ApplicationDbContext DbContext;
    protected readonly IFixture Fixture;
    protected readonly Random Random = new Random();

    private readonly Func<Task> _resetDatabase;

    protected TestsBase(IntegrationsTestsWebAppFactory factory)
    {
        Factory = factory;
        _resetDatabase = factory.ResetDatabaseAsync;
        Scope = factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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