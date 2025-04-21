using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

public class SocialNetworks : BaseCollectionVO<SocialNetwork, SocialNetworks>
{
    public const int MAX_SOCIAL_COUNT = 5;

    private SocialNetworks(IEnumerable<SocialNetwork> items) : base(items)
    {
    }

    private SocialNetworks()
    {
    }

    public static Result<SocialNetworks, Error> Create(IEnumerable<SocialNetwork> items)
    {
        var enumerable = items.ToList();
        if (enumerable.Count > MAX_SOCIAL_COUNT)
        {
            return ErrorsPreform.General.Validation("SocialNetworks count can't be more than 10.",
                nameof(SocialNetworks));
        }

        return new SocialNetworks(enumerable);
    }

    public static Result<SocialNetworks, Error> CreateEmpty()
    {
        IEnumerable<SocialNetwork> socialNetworks = [];
        return new SocialNetworks(socialNetworks);
    }
}