using PetFamily.Shared.Errors.Enums;

namespace PetFamily.Shared.Errors;

public class Error
{
    private const int LENGHTFORMESSAGE = 25;

    public string Message { get; }

    public ErrorType ErrorType { get; }

    public string? InvalidField { get; }

    private Error(string message, ErrorType errorType, string? invalidField)
    {
        Message = message;
        ErrorType = errorType;
        InvalidField = invalidField;
    }

    public override string ToString()
    {
        var messageLength = Message.Length > LENGHTFORMESSAGE
            ? LENGHTFORMESSAGE * (int)Math.Ceiling((double)Message.Length / LENGHTFORMESSAGE)
            : LENGHTFORMESSAGE;

        return string.Format(
            "{0" + "}: {1,-" + messageLength + "} {2" + "}",
            ErrorType.ToString(),
            Message,
            InvalidField ?? string.Empty
        );
    }

    public static Error Create(string message, ErrorType errorType, string? invalidField)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        return new Error(message, errorType, invalidField);
    }

    public static Error Validation(string message, string invalidField)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(invalidField);

        return new Error(message, ErrorType.VALIDATION, invalidField);
    }
}