using PetFamily.Shared.Errors.Enums;

namespace PetFamily.Shared.Errors;

public static class ErrorsPreform
{
    public static class General
    {
        public static Error Validation(string message, string invalidField)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(message);
            ArgumentException.ThrowIfNullOrWhiteSpace(invalidField);

            return Error.Create(message, ErrorCodes.General.InvalidField, ErrorType.VALIDATION, invalidField);
        }

        public static Error NotFound(Guid? id = null)
        {
            if (id == Guid.Empty)
            {
                throw new Exception("Invalid ID");
            }

            return Error.Create($"{id} not found", ErrorCodes.General.NotFound, ErrorType.NOTFOUND);
        }

        public static Error None()
        {
            return Error.Create(string.Empty, string.Empty, ErrorType.NONE, string.Empty);
        }
    }
}