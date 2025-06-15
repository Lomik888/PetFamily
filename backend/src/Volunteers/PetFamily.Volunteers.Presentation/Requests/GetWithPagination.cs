using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Queries.Get;

namespace PetFamily.Volunteers.Presentation.Requests;

public class GetWithPagination : IToQuery<GetQuery>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public GetQuery ToQuery()
    {
        return new GetQuery(Page, PageSize);
    }
}