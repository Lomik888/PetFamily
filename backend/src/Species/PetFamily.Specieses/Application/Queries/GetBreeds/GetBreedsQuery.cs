using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Specieses.Application.Queries.GetBreeds;

public record GetBreedsQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;