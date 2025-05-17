using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Queries.Get;

public record GetQuery(int Page, int PageSize) : IQuery;