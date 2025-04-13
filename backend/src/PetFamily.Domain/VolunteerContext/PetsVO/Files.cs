using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Shared.Errors;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class Files : BaseCollectionVO<File, Files>
{
    private const int MAX_FILE_COUNT = 10;

    private Files(IEnumerable<File> items) : base(items)
    {
    }

    private Files()
    {
    }

    public override Result<Files, Error> Create(IEnumerable<File> items)
    {
        var enumerable = items.ToList();
        if (enumerable.Count > MAX_FILE_COUNT)
        {
            return Error.Validation("Files count can't be more than 10.", nameof(Files));
        }

        return new Files(enumerable);
    }
}