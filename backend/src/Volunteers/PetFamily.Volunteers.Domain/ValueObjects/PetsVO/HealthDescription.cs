﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

public class HealthDescription : ValueObject
{
    public const int MIN_LENGHT = 1;
    public const int SHAREDHEALTHSTATUS_MAX_LENGHT = 500;
    public const int SKINCONDITION_MAX_LENGHT = 500;
    public const int MOUTHCONDITION_MAX_LENGHT = 500;
    public const int DIGESTIVESYSTEMCONDITION_MAX_LENGHT = 500;

    public string SharedHealthStatus { get; }
    public string SkinCondition { get; }
    public string MouthCondition { get; }
    public string DigestiveSystemCondition { get; }

    private HealthDescription(
        string sharedHealthStatus,
        string skinCondition,
        string mouthCondition,
        string digestiveSystemCondition)
    {
        SharedHealthStatus = sharedHealthStatus;
        SkinCondition = skinCondition;
        MouthCondition = mouthCondition;
        DigestiveSystemCondition = digestiveSystemCondition;
    }

    public static Result<HealthDescription, ErrorList> Create(
        string sharedHealthStatus,
        string skinCondition,
        string mouthCondition,
        string digestiveSystemCondition)
    {
        var errors = new List<Error>();

        Validator.FieldValueObject.Validation(sharedHealthStatus, MIN_LENGHT, SHAREDHEALTHSTATUS_MAX_LENGHT, errors);
        Validator.FieldValueObject.Validation(skinCondition, MIN_LENGHT, SKINCONDITION_MAX_LENGHT, errors);
        Validator.FieldValueObject.Validation(mouthCondition, MIN_LENGHT, MOUTHCONDITION_MAX_LENGHT, errors);
        Validator.FieldValueObject.Validation(
            digestiveSystemCondition,
            MIN_LENGHT,
            DIGESTIVESYSTEMCONDITION_MAX_LENGHT,
            errors);

        if (errors.Count > 0)
        {
            return ErrorList.Create(errors);
        }

        return new HealthDescription(
            sharedHealthStatus,
            skinCondition,
            mouthCondition,
            digestiveSystemCondition);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SharedHealthStatus;
        yield return SkinCondition;
        yield return MouthCondition;
        yield return DigestiveSystemCondition;
    }
}