﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class HelpStatus : ValueObject
{
    public HelpStatuses Value { get; }

    private HelpStatus(HelpStatuses value)
    {
        Value = value;
    }

    public static Result<HelpStatus, Error> Create(HelpStatuses value)
    {
        if (Enum.IsDefined(typeof(HelpStatuses), value) == false)
        {
            return ErrorsPreform.General.Validation("invalid help status", nameof(HelpStatus));
        }

        return new HelpStatus(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}