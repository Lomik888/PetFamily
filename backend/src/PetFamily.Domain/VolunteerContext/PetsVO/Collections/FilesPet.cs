using CSharpFunctionalExtensions;
using PetFamily.Domain.Contracts;
using PetFamily.Shared.Errors;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Domain.VolunteerContext.PetsVO.Collections;

public class FilesPet : ValueObjectList<File>
{
    private const int MAX_FILE_COUNT = 10;

    private FilesPet(IEnumerable<File> items) : base(items)
    {
    }

    private FilesPet()
    {
    }
    
    public static Result<FilesPet, Error> Create(IEnumerable<File> items)
    {
        var enumerable = items.ToList();
        if (enumerable.Count > MAX_FILE_COUNT)
        {
            return ErrorsPreform.General.Validation("Files count can't be more than 10.", nameof(FilesPet));
        }

        return new FilesPet(enumerable);
    }

    public static Result<FilesPet> CreateEmpty()
    {
        IEnumerable<File> files = [];
        return new FilesPet(files);
    }
}