using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Queries.Get;

public record GetQuery(int Page, int PageSize) : IQuery;