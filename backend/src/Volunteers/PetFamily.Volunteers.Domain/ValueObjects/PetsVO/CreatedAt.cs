﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

public class CreatedAt : ValueObject
{
    private static readonly DateTime VALUE_DATE_AFTER_INVALID = new DateTime(2000, 1, 26);

    public DateTime Value { get; }

    private CreatedAt(DateTime value)
    {
        Value = value;
    }

    public static Result<CreatedAt, Error> Create(DateTime value)
    {
        if (value > DateTime.UtcNow || value < VALUE_DATE_AFTER_INVALID || value.Kind != DateTimeKind.Utc)
        {
            return ErrorsPreform.General.Validation("Date created is invalid", nameof(CreatedAt));
        }

        return new CreatedAt(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}