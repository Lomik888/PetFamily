using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Infrastructure.DependencyInjection;
using PetFamily.Framework;
using PetFemily.Accounts.Application.DependencyInjection;

namespace PetFamily.Accounts.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static void AddAccountsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureAuthorization(configuration);
        services.AddAccountsApplicationLayer();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddAuthorization();
        services.AddAuthentication(configuration);
    }

    private static void AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options => { });
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
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
}