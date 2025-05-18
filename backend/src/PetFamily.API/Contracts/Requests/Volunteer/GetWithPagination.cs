using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Queries.Get;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public class GetWithPagination : IToQuery<GetQuery>
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public GetQuery ToQuery()
    {
        return new GetQuery(Page, PageSize);
    }
}