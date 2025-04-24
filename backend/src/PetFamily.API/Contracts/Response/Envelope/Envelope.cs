namespace PetFamily.API.Response.Envelope;

public record Envelope
{
    public object? Data { get; }
    public IEnumerable<ErrorResponse>? Errors { get; }
    public DateTime CreatedAt => DateTime.UtcNow;

    private Envelope(object? data, IEnumerable<ErrorResponse>? errors)
    {
        Data = data;
        Errors = errors;
    }

    public static Envelope Ok(object data) => new(data, null);
    public static Envelope OkEmpty() => new(null, null);
    public static Envelope Error(IEnumerable<ErrorResponse> errors) => new(null, errors);
    public static Envelope Error(ErrorResponse errors) => new(null, new[] { errors });
}