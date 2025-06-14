using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Specieses.Domain;
using PetFamily.Specieses.Infrastructure.Database;
using PetFamily.Specieses.Marker;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Infrastructure.DbContext;
using PetFamily.Volunteers.Infrastructure.Marker;

namespace PetFamily.Data.Tests;

public class TestDbContext : DbContext
{
    public DbSet<Volunteer> Volunteers { get; init; }
    public DbSet<Species> Specieses { get; init; }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(build => build.AddConsole()));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SpeciesDbContext).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerDbContext).Assembly);
    }
}