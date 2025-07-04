﻿using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using NSubstitute;
using PetFamily.API;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Dtos;
using PetFamily.Data.Tests.Database;
using PetFamily.FileService.BackgroundWorkers;
using PetFamily.Specieses.Infrastructure;
using PetFamily.Specieses.Infrastructure.Database;
using PetFamily.Volunteers.Infrastructure.BackgroundWorkers;
using PetFamily.Volunteers.Infrastructure.DbContext;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetFamily.Application.IntegrationTests;

public class IntegrationsTestsWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("1332132_pet_family_tests")
        .WithUsername("rootroot")
        .WithPassword("rootroot")
        .Build();

    private readonly IFilesProvider _fileProviderMock = Substitute.For<IFilesProvider>();
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

        services.RemoveAll<SpeciesDbContext>();
        services.RemoveAll<VolunteerDbContext>();
        services.RemoveAll<DbContextOptions<SpeciesDbContext>>();
        services.RemoveAll<DbContextOptions<VolunteerDbContext>>();

        services.AddDbContext<SpeciesDbContext>(x => x.UseNpgsql(_postgreSqlContainer.GetConnectionString()));
        services.AddDbContext<VolunteerDbContext>(x => x.UseNpgsql(_postgreSqlContainer.GetConnectionString()));
        services.AddDbContext<TestDbContext>(x => x.UseNpgsql(_postgreSqlContainer.GetConnectionString()));

        services.RemoveAll(typeof(ILogger<>));
        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));

        services.RemoveAll<IFilesProvider>();
        services.AddScoped<IFilesProvider>(_ => _fileProviderMock);
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            SchemasToInclude = ["public", "Species", "Volunteers"],
            DbAdapter = DbAdapter.Postgres
        });
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();

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

    public void FileProviderSuccessUploadAsync()
    {
        var filePath = "someFilePathUploaded";

        _fileProviderMock.UploadAsync(
                Arg.Any<FilePathDto>(),
                Arg.Any<Stream>(),
                Arg.Any<CancellationToken>())
            .Returns(filePath);
    }

    public void FileProviderSuccessRemoveAsync()
    {
        var filePath = "someFilePathDeleted";

        _fileProviderMock.RemoveAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(filePath);
    }

    public void FileProviderSuccessPredefinedGetAsync()
    {
        var filePath = "someFilePathLink";

        _fileProviderMock.PredefinedGetAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(filePath);
    }
}