using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.SpeciesUseCases.Queries.GetBreeds;

namespace PetFamily.API.Contracts.Requests.Species;

public class GetBreedsWithPaginationRequest : IToQuery<GetBreedsQuery, Guid>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public GetBreedsQuery ToQuery(Guid speciesId)
    {
        return new GetBreedsQuery(speciesId, Page, PageSize);
    }
}