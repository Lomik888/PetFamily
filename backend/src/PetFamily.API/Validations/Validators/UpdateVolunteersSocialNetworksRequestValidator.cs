using FluentValidation;
using PetFamily.API.Contracts.Requests.Volunteer;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

namespace PetFamily.API.Validations.Validators;

public class UpdateVolunteersSocialNetworksRequestValidator : AbstractValidator<UpdateVolunteersSocialNetworksRequest>
{
    public UpdateVolunteersSocialNetworksRequestValidator()
    {
        RuleForEach(x => x.SocialNetworksDto)
            .MustBeValueObject(x => SocialNetwork.Create(x.Title, x.Url));

        RuleFor(x => x.SocialNetworksDto)
            .Must(x => x.Count <= SocialNetworks.MAX_SOCIAL_COUNT)
            .WithMessageCustom("SocialNetworks count can't be greater than maximum");
    }
}