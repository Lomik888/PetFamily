using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.SpeciesUseCases.Queries.GetSpecies;
namespace PetFamily.API.Contracts.Requests.Species;

public class GetWithPaginationSpeciesRequest : IToQuery<GetSpeciesQuery>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public GetSpeciesQuery ToQuery()
    {
        return new GetSpeciesQuery(Page, PageSize);
    }
}