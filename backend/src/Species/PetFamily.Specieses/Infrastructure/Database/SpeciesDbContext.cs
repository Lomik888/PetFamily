using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Specieses.Domain;

namespace PetFamily.Specieses.Infrastructure.Database;

public class SpeciesDbContext : DbContext
{
    public DbSet<Species> Species { get; init; }

    public SpeciesDbContext(DbContextOptions<SpeciesDbContext> options) : base(options)
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
    }
}