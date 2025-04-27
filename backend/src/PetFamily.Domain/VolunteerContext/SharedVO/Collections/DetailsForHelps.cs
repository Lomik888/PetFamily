using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.SharedVO.Collections;

public class DetailsForHelps : BaseCollectionVO<DetailsForHelp, DetailsForHelps>
{
    public const int MAX_DETAILS_COUNT = 10;

    private DetailsForHelps(IEnumerable<DetailsForHelp> items) : base(items)
    {
    }

    private DetailsForHelps()
    {
    }

    public static Result<DetailsForHelps, Error> Create(IEnumerable<DetailsForHelp> items)
    {
        var enumerable = items.ToList();
        if (enumerable.Count > MAX_DETAILS_COUNT)
        {
            return ErrorsPreform.General.Validation("Details count can't be more than 10.", nameof(DetailsForHelps));
        }

        return new DetailsForHelps(enumerable);
    }

    public static Result<DetailsForHelps> CreateEmpty()
    {
        IEnumerable<DetailsForHelp> detailsForHelp = [];
        return new DetailsForHelps(detailsForHelp);
    }
}