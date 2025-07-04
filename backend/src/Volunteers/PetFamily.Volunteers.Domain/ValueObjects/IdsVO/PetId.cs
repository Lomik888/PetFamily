﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

public class PetId : ValueObject, IComparable<PetId>
{
    public Guid Value { get; }

    private PetId(Guid value)
    {
        Value = value;
    }

    public static Result<PetId, Error> Create()
    {
        return new PetId(Guid.NewGuid());
    }

    public static Result<PetId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Pet id invalid", nameof(PetId));
        }

        return new PetId(id);
    }

    public static Result<PetId> CreateEmpty()
    {
        return new PetId(Guid.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public int CompareTo(PetId? other)
    {
        if (object.ReferenceEquals(other, null))
        {
            return 1;
        }

        return (object)this == (object)other ? 0 : this.Value.CompareTo(other.Value);
    }
}