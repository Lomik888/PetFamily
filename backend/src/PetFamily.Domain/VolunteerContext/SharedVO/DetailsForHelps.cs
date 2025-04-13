using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.SharedVO;

public class DetailsForHelps : BaseCollectionVO<DetailsForHelp, DetailsForHelps>
{
    private const int MAX_DETAILS_COUNT = 10;

    private DetailsForHelps(IEnumerable<DetailsForHelp> items) : base(items)
    {
    }

    private DetailsForHelps()
    {
    }

    public override Result<DetailsForHelps, Error> Create(IEnumerable<DetailsForHelp> items)
    {
        var enumerable = items.ToList();
        if (enumerable.Count > MAX_DETAILS_COUNT)
        {
            return Error.Validation("Details count can't be more than 10.", nameof(DetailsForHelps));
        }

        return new DetailsForHelps(enumerable);
    }
}