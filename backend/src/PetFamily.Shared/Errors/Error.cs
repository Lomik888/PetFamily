using PetFamily.Shared.Errors.Enums;

namespace PetFamily.Shared.Errors;

public class Error
{
    private const int LENGHTFORMESSAGE = 25;

    public string? ErrorCode { get; }

    public string? Message { get; }

    public ErrorType ErrorType { get; }

    public string? InvalidField { get; }

    private Error(string? message, string? errorCode, ErrorType errorType, string? invalidField)
    {
        Message = message;
        ErrorType = errorType;
        InvalidField = invalidField;
        ErrorCode = errorCode;
    }

    public override string ToString()
    {
        var messageLength = Message?.Length > LENGHTFORMESSAGE
            ? LENGHTFORMESSAGE * (int)Math.Ceiling((double)Message.Length / LENGHTFORMESSAGE)
            : LENGHTFORMESSAGE;

        return string.Format(
            "{0" + "}:{1} {2,-" + messageLength + "} {3" + "}",
            ErrorType.ToString(),
            ErrorCode,
            Message,
            InvalidField ?? string.Empty
        );
    }

    public static Error Create(string? message, string errorCode, ErrorType errorType, string? invalidField = null)
    {
        return new Error(message, errorCode, errorType, invalidField);
    }
}