using CSharpFunctionalExtensions;
using PetFamily.Domain.Contracts;
using PetFamily.Shared.Errors;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Domain.VolunteerContext.PetsVO.Collections;

public class Files : ValueObjectList<File>
{
    private const int MAX_FILE_COUNT = 10;

    private Files(IEnumerable<File> items) : base(items)
    {
    }

    private Files()
    {
    }
    
    public static Result<Files, Error> Create(IEnumerable<File> items)
    {
        var enumerable = items.ToList();
        if (enumerable.Count > MAX_FILE_COUNT)
        {
            return ErrorsPreform.General.Validation("Files count can't be more than 10.", nameof(Files));
        }

        return new Files(enumerable);
    }

    public static Result<Files> CreateEmpty()
    {
        IEnumerable<File> files = [];
        return new Files(files);
    }
}