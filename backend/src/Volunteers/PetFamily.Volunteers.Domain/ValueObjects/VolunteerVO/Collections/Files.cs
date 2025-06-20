﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.ValueObjects;
using File = PetFamily.SharedKernel.ValueObjects.File;
using SharedVO_File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO.Collections;

public class Files : ValueObjectList<SharedVO_File>
{
    private Files(IEnumerable<File> items) : base(items)
    {
    }

    private Files()
    {
    }

    public static Result<Files> Create(IEnumerable<File> items)
    {
        return new Files(items);
    }

    public static Result<Files> CreateEmpty()
    {
        IEnumerable<File> files = [];
        return new Files(files);
    }
}