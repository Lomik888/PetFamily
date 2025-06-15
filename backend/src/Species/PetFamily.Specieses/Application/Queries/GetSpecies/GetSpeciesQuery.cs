using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Specieses.Application.Queries.GetSpecies;

public record GetSpeciesQuery(int Page, int PageSize) : IQuery;