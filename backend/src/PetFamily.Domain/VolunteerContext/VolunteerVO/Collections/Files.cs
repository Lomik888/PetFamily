using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Shared.Errors;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

public class Files : BaseCollectionVO<File, Files>
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