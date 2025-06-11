namespace PetFamily.Framework.Responses;

public record ErrorResponse(string? Code, string? Message, string ErrorType);