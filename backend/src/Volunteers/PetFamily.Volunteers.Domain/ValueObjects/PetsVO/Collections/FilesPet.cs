using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;
using File = PetFamily.Volunteers.Domain.ValueObjects.SharedVO.File;
using SharedVO_File = PetFamily.Volunteers.Domain.ValueObjects.SharedVO.File;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Collections;

public class FilesPet : ValueObjectList<SharedVO_File>
{
    public const int MAX_FILE_COUNT = 10;

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