using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Shared.Errors;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public class Files : BaseCollectionVO<File, Files>
{
    private Files(IEnumerable<File> items) : base(items)
    {
    }

    private Files()
    {
    }

    public override Result<Files, Error> Create(IEnumerable<File> items)
    {
        return new Files(items);
    }
}