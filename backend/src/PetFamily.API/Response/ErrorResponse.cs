namespace PetFamily.API.Response;

public record ErrorResponse(string? Code, string? Message, string ErrorType);