using PetFamily.Framework.Abstractions;
using PetFamily.Specieses.Application.Queries.GetSpecies;

namespace PetFamily.Specieses.Presentation.Requests;

public class GetWithPaginationSpeciesRequest : IToQuery<GetSpeciesQuery>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public GetSpeciesQuery ToQuery()
    {
        return new GetSpeciesQuery(Page, PageSize);
    }
}