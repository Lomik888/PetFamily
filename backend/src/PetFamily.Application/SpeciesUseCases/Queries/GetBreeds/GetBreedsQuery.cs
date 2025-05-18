using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.SpeciesUseCases.Queries.GetBreeds;

public record GetBreedsQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;