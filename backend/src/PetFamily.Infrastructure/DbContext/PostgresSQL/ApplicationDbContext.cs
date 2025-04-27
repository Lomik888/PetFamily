using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.VolunteerContext.Entities;

namespace PetFamily.Infrastructure.DbContext.PostgresSQL;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Volunteer> Volunteers { get; init; }
    public DbSet<Species> Species { get; init; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}