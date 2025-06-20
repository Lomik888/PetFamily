using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO.Collections;

namespace PetFamily.Volunteers.Application.Commands.UpdateSocialNetworks;

public class UpdateVolunteersSocialNetworksCommandValidator : AbstractValidator<UpdateVolunteersSocialNetworksCommand>
{
    public UpdateVolunteersSocialNetworksCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));

        // RuleForEach(x => x.SocialNetworkCollectionDto.SocialNetworks)
        //     .MustBeValueObject(x => SocialNetwork.Create(x.Title, x.Url));
        //
        // RuleFor(x => x.SocialNetworkCollectionDto.SocialNetworks)
        //     .Must(x => x.Count <= SocialNetworks.MAX_SOCIAL_COUNT)
        //     .WithMessageCustom("SocialNetworks count can't be greater than maximum");
    }
}