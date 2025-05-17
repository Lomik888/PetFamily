namespace PetFamily.Application.VolunteerUseCases.Queries;

public class GetObjectsWithPaginationResponse<T> where T : class
{
    public T[]? Data { get; set; }
    public long Count { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}