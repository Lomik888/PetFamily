using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

namespace PetFamily.Application.VolunteerUseCases.Commands.UpdateSocialNetworks;

public class UpdateVolunteersSocialNetworksCommandValidator : AbstractValidator<UpdateVolunteersSocialNetworksCommand>
{
    public UpdateVolunteersSocialNetworksCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));

        RuleForEach(x => x.SocialNetworkCollectionDto.SocialNetworks)
            .MustBeValueObject(x => SocialNetwork.Create(x.Title, x.Url));

        RuleFor(x => x.SocialNetworkCollectionDto.SocialNetworks)
            .Must(x => x.Count <= SocialNetworks.MAX_SOCIAL_COUNT)
            .WithMessageCustom("SocialNetworks count can't be greater than maximum");
    }
}