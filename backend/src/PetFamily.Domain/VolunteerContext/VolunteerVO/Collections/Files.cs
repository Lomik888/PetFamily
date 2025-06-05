using CSharpFunctionalExtensions;
using PetFamily.Domain.Contracts;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

public class Files : ValueObjectList<File>
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