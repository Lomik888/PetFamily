using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using PetFamily.Application.BackgroundWorkers.HardDeleteWorker;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.BackgroundWorkers;
using PetFamily.Infrastructure.DbContext.PostgresSQL;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetFamily.Application.IntegrationTests;

public class IntegrationsTestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("my_pet_family_tests")
        .WithUsername("rootroot")
        .WithPassword("rootroot")
        .Build();

    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var deleteInvalidFilesWorker =
            services.SingleOrDefault(x => x.ImplementationType == typeof(DeleteInvalidFilesWorker));
        if (deleteInvalidFilesWorker != null)
            services.Remove(deleteInvalidFilesWorker);

        var hardDeleteUnActiveEntitiesWorker =
            services.SingleOrDefault(x => x.ImplementationType == typeof(HardDeleteUnActiveEntitiesWorker));
        if (hardDeleteUnActiveEntitiesWorker != null)
            services.Remove(hardDeleteUnActiveEntitiesWorker);

        services.RemoveAll<ISqlConnectionFactory>();
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
            new SqlConnectionFactory(_postgreSqlContainer.GetConnectionString()));

        services.RemoveAll<ApplicationDbContext>();
        services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
        });

        services.RemoveAll(typeof(ILogger<>));
        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            SchemasToInclude = ["public"],
            DbAdapter = DbAdapter.Postgres
        });
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        _dbConnection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString());
        await InitializeRespawnerAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
        await _postgreSqlContainer.DisposeAsync();
    }
}