using PetFamily.Shared.Errors.Enums;
using PetFamily.Shared.Errors.Interfaces;

namespace PetFamily.Shared.Errors;

public class Error : IError
{
    private const int LENGHTFORMESSAGE = 25;
    private const string SEPARATOR = " || ";
    private const int FIELD_SERILIZED_ERROR = 3;

    public string? ErrorCode { get; }

    public string? Message { get; }

    public ErrorType ErrorType { get; }

    public string? InvalidField { get; }

    private Error(
        string? message,
        string? errorCode,
        ErrorType errorType,
        string? invalidField)
    {
        Message = message;
        ErrorType = errorType;
        InvalidField = invalidField;
        ErrorCode = errorCode;
    }

    public override string ToString()
    {
        var messageLength =
            Message?.Length > LENGHTFORMESSAGE
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

    public static Error Create(
        string? message,
        string? errorCode,
        ErrorType errorType,
        string? invalidField = null)
    {
        if (Enum.IsDefined(typeof(ErrorType), errorType) == false)
            throw new FormatException("Invalid error type");

        return new Error(message, errorCode, errorType, invalidField);
    }

    public string Serialize()
    {
        return string.Join(SEPARATOR, ErrorCode, Message, ErrorType);
    }

    public static Error Deserialize(string serializedError)
    {
        if (string.IsNullOrWhiteSpace(serializedError))
            throw new ArgumentNullException(nameof(serializedError), "serializedError can't be null or white space");

        var fields = serializedError.Split(SEPARATOR);

        if (fields.Length != FIELD_SERILIZED_ERROR)
            throw new FormatException("Invalid error format");

        if (!Enum.TryParse(fields[2], out ErrorType errorType))
            throw new FormatException("Invalid error format");

        return new Error(fields[1], fields[0], errorType, null);
    }
}