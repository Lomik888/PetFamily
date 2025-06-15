using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure;

public class AccountDbContext : IdentityDbContext<User, Role, Guid>
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(build => build.AddConsole()));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().ToTable("users", schema: "Accounts");
        builder.Entity<Role>().ToTable("roles", schema: "Accounts");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims", schema: "Accounts");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens", schema: "Accounts");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins", schema: "Accounts");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims", schema: "Accounts");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles", schema: "Accounts");
    }
}