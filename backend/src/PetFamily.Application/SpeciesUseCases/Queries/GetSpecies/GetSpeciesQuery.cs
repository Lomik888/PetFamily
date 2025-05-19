using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.SpeciesUseCases.Queries.GetSpecies;

public record GetSpeciesQuery(int Page, int PageSize) : IQuery;