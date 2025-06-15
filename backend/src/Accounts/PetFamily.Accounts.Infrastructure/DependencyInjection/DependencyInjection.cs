using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Providers;
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
        services.AddAuthentication(configuration);
        services.AddAuthorization();
        services.AddProviders();
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

    private static void AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options => { });
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetRequiredSection("Jwt");
        var audience = jwtSection.GetValue<string>("Audience");
        var issuer = jwtSection.GetValue<string>("Issuer");
        var securityKey = jwtSection.GetValue<string>("SecurityKey") ??
                          throw new NullReferenceException("SecurityKey is missing");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
            {
                jwtOptions.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            success = false,
                            errors = new[] { "Unauthorized" },
                            message = "Unauthorized"
                        };

                        return context.Response.WriteAsJsonAsync(response);
                    }
                };

                jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                };
            });
    }

    private static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetRequiredSection("Jwt"));
    }
}