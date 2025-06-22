using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Providers;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Enums;
using PetFamily.Framework;
using PetFemily.Accounts.Application;
using PetFemily.Accounts.Application.Providers;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);
        services.AddDatabase(configuration);
        services.AddIdentity();
        services.AddProviders();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(UnitOfWorkTypes.Accounts);
        services.AddScoped<IAccountManager, AccountManager>();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlSection = configuration.GetRequiredSection("Psql_Connection_String");
        var connectionString = psqlSection.GetValue<string>("Postgres_SQL") ??
                               throw new NullReferenceException("Postgres SQL Connection String missing");

        services.AddDbContext<AccountDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void AddProviders(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokensProvider, JwtTokensProvider>();
    }

    private static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            })
            .AddEntityFrameworkStores<AccountDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetRequiredSection("Jwt"));
    }
}