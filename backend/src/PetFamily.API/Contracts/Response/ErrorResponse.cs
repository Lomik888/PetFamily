namespace PetFamily.API.Contracts.Response;

public record ErrorResponse(string? Code, string? Message, string ErrorType);