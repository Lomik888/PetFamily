using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public class SocialNetworks : BaseCollectionVO<SocialNetwork, SocialNetworks>
{
    public const int MAX_SOCIAL_COUNT = 5;

    private SocialNetworks(IEnumerable<SocialNetwork> items) : base(items)
    {
    }

    private SocialNetworks()
    {
    }

    public override Result<SocialNetworks, Error> Create(IEnumerable<SocialNetwork> items)
    {
        var enumerable = items.ToList();
        if (enumerable.Count > MAX_SOCIAL_COUNT)
        {
            return Error.Validation("SocialNetworks count can't be more than 10.", nameof(SocialNetworks));
        }

        return new SocialNetworks(enumerable);
    }
}