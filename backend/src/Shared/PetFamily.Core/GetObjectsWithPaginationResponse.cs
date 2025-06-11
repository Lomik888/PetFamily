namespace PetFamily.Core;

public class GetObjectsWithPaginationResponse<T> where T : class
{
    public IEnumerable<T>? Data { get; set; }
    public long Count { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}