using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Specieses.Application.Abstractions;
using PetFamily.Specieses.Contracts;
using PetFamily.Specieses.Infrastructure.Repositories;
using PetFamily.Specieses.Presentation.Contracts;

namespace PetFamily.Specieses.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static void AddSpeciesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<ISpeciesExistenceContracts, SpeciesExistenceContracts>();
    }
}