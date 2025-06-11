using PetFamily.Framework.Abstractions;
using PetFamily.Specieses.Application.Queries.GetBreeds;
using PetFamily.Specieses.Application.Queries.GetBreeds;

namespace PetFamily.Specieses.Presentation.Requests;

public class GetBreedsWithPaginationRequest : IToQuery<GetBreedsQuery, Guid>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public GetBreedsQuery ToQuery(Guid speciesId)
    {
        return new GetBreedsQuery(speciesId, Page, PageSize);
    }
}