using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Infrastructure.DependencyInjection;
using PetFemily.Accounts.Application.DependencyInjection;

namespace PetFamily.Accounts.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static void AddAccountsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureAuthorization(configuration);
        services.AddAccountsApplicationLayer();
    }
}