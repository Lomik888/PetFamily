using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFemily.Accounts.Application;
using PetFemily.Accounts.Application.Dto;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class ReadAccountDbContext : DbContext, IReadDbContext
{
    public IQueryable<User> Users => Set<User>();
    public IQueryable<Role> Roles => Set<Role>();
    public IQueryable<Permission> Permissions => Set<Permission>();
    public IQueryable<AdminAccount> AdminAccount => Set<AdminAccount>();
    public IQueryable<VolunteerAccount> VolunteerAccount => Set<VolunteerAccount>();
    public IQueryable<ParticipantAccount> ParticipantAccount => Set<ParticipantAccount>();
    public IQueryable<RefreshSessions> RefreshSessions => Set<RefreshSessions>();

    public ReadAccountDbContext(DbContextOptions<ReadAccountDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.EnableSensitiveDataLogging();
        //optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(build => build.AddConsole()));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Accounts");
        builder.ApplyConfigurationsFromAssembly(typeof(ReadAccountDbContext).Assembly);
    }

    public override int SaveChanges()
        => throw new NotImplementedException("ReadDbContext can't save.");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        => throw new NotImplementedException("ReadDbContext can't save.");

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
        => throw new NotImplementedException("ReadDbContext can't save.");

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
        => throw new NotImplementedException("ReadDbContext can't save.");
}