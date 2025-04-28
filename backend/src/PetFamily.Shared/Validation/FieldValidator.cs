using System.Runtime.CompilerServices;
using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Shared.Validation;

public static class Validator
{
    public static class FieldValueObject
    {
        public static void Validation(
            string fieldValue,
            int minLenght,
            int maxLenght,
            List<Error> errors,
            [CallerArgumentExpression("fieldValue")]
            string fieldName = "")
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                errors.Add(ErrorsPreform.General.Validation(
                    $"{fieldName} is required",
                    fieldName));
                return;
            }

            if (fieldValue.Length < minLenght || fieldValue.Length > maxLenght)
            {
                errors.Add(ErrorsPreform.General.Validation(
                    $"{fieldName} min length is {minLenght} and max length is {maxLenght}",
                    fieldName));
            }
        }

        public static void Validation(
            string fieldValue,
            int minLenght,
            List<Error> errors,
            [CallerArgumentExpression("fieldValue")]
            string fieldName = "")
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                errors.Add(ErrorsPreform.General.Validation(
                    $"{fieldName} is required",
                    fieldName));
                return;
            }

            if (fieldValue.Length < minLenght)
            {
                errors.Add(ErrorsPreform.General.Validation(
                    $"{fieldName} min length is {minLenght}",
                    fieldName));
            }
        }

        public static UnitResult<Error> ValidationNumber<T>(
            T fieldValue,
            bool isNegativeValue,
            int? decimalPrecision,
            T maxValue,
            [CallerArgumentExpression("fieldValue")]
            string fieldName = "") where T : struct, IComparable<T>
        {
            if (fieldValue.CompareTo(maxValue) > 0 ||
                !isNegativeValue && fieldValue.CompareTo(default) < 0)
            {
                return UnitResult.Failure(ErrorsPreform.General.Validation(
                    $"{fieldName} can't be negative and max value is {maxValue}",
                    fieldName));
            }

            if (decimalPrecision != null && fieldValue.ToString()!.Split(',', '.').Last().Length > decimalPrecision)
            {
                return UnitResult.Failure(ErrorsPreform.General.Validation(
                    $"{fieldName} decimal precision is more than {decimalPrecision}",
                    fieldName));
            }

            switch (fieldValue)
            {
                case float value:
                    if (float.IsNaN(value) || float.IsInfinity(value))
                    {
                        throw new ArgumentException($"{fieldName} can't be NaN or Infinity");
                    }

                    break;

                case double value:
                    if (double.IsNaN(value) || double.IsInfinity(value))
                    {
                        throw new ArgumentException($"{fieldName} can't be NaN or Infinity");
                    }

                    break;
            }

            return UnitResult.Success<Error>();
        }

        public static UnitResult<Error> Validation(
            string fieldValue,
            int minLenght,
            int maxLenght,
            [CallerArgumentExpression("fieldValue")]
            string fieldName = "")
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                return UnitResult.Failure(ErrorsPreform.General.Validation(
                    $"{fieldName} is required",
                    fieldName));
            }

            if (fieldValue.Length < minLenght || fieldValue.Length > maxLenght)
            {
                return UnitResult.Failure(ErrorsPreform.General.Validation(
                    $"{fieldName} min length is {minLenght} and max length is {maxLenght}",
                    fieldName));
            }

            return UnitResult.Success<Error>();
        }

        public static void ValidationNullable(
            string? fieldValue,
            int minLenght,
            int maxLenght,
            List<Error> errors,
            [CallerArgumentExpression("fieldValue")]
            string fieldName = "")
        {
            if (fieldValue == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                errors.Add(ErrorsPreform.General.Validation(
                    $"{fieldName} can't be whiteSpace",
                    fieldName));
                return;
            }

            if (fieldValue.Length < minLenght || fieldValue.Length > maxLenght)
            {
                errors.Add(ErrorsPreform.General.Validation(
                    $"{fieldName} min length is {minLenght} and max length is {maxLenght}",
                    fieldName));
            }
        }

        public static Result ValidationNullable(
            string? fieldValue,
            int minLenght,
            int maxLenght,
            [CallerArgumentExpression("fieldValue")]
            string fieldName = "")
        {
            if (fieldValue is not null)
            {
                return Result.Success();
            }

            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                return Result.Failure(ErrorsPreform.General.Validation(
                    $"{fieldName} can't be whiteSpace",
                    fieldName).Serialize());
            }

            if (fieldValue.Length < minLenght || fieldValue.Length > maxLenght)
            {
                return Result.Failure(ErrorsPreform.General.Validation(
                    $"{fieldName} min length is {minLenght} and max length is {maxLenght}",
                    fieldName).Serialize());
            }

            return Result.Success();
        }
    }

    public static class Guard
    {
        public static void NotNull<T>(
            T value,
            [CallerArgumentExpression("value")] string valueName = "")
            where T : class
        {
            if (value is null)
                throw new ArgumentNullException(valueName, $"{valueName} cannot be null");
        }

        public static void NotEmpty(
            Guid value,
            [CallerArgumentExpression("value")] string valueName = "")
        {
            if (value == Guid.Empty)
                throw new ArgumentException(valueName, $"{valueName} can't be empty");
        }

        public static void IsUtc(
            DateTime value,
            [CallerArgumentExpression("value")] string valueName = "")
        {
            if (value.Kind != DateTimeKind.Utc)
                throw new ArgumentException(valueName, $"{valueName} is not a UTC");
        }

        public static void IsPastRetentionPeriod(
            DateTime validateValue,
            int addMinutesDelay,
            [CallerArgumentExpression("validateValue")]
            string valueName = "")
        {
            IsUtc(validateValue);

            var utcNow = DateTime.UtcNow;

            if (validateValue > utcNow || validateValue < utcNow.AddMinutes(addMinutesDelay))
                throw new ArgumentException(valueName, $"{valueName} date can't be before last period");
        }
    }
}