using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure;

public class ReadAccountDbContext : DbContext
{
    public new DbSet<UserDto> Users { get; set; }
    public new DbSet<RoleDto> Roles { get; set; }
    public DbSet<PermissionDto> Permissions { get; set; }
    public DbSet<AdminAccountDto> AdminAccount { get; set; }
    public DbSet<VolunteerAccountDto> VolunteerAccount { get; set; }
    public DbSet<ParticipantAccountDto> ParticipantAccount { get; set; }
    public DbSet<RefreshSessionsDto> RefreshSessions { get; set; }

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
        base.OnModelCreating(builder);
        // builder.Entity<IdentityUserRole<Guid>>().ToTable("users_roles", schema: "Accounts");
        // builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims", schema: "Accounts");
        // builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens", schema: "Accounts");
        // builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins", schema: "Accounts");
        // builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims", schema: "Accounts");
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