using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Volunteers.Domain;

namespace PetFamily.Volunteers.Infrastructure.DbContext;

public class VolunteerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Volunteer> Volunteers { get; init; }

    public VolunteerDbContext(DbContextOptions<VolunteerDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(build => build.AddConsole()));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VolunteerDbContext).Assembly);
    }
}