using FluentValidation;
using PetFamily.API.Requests.Volunteer;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;

namespace PetFamily.API.Validations.Validators;

public class UpdateVolunteersDetailsForHelpRequestValidator : AbstractValidator<UpdateVolunteersDetailsForHelpRequest>
{
    public UpdateVolunteersDetailsForHelpRequestValidator()
    {
        RuleForEach(x => x.DetailsForHelps)
            .MustBeValueObject(x => DetailsForHelp.Create(x.Title, x.Description));

        RuleFor(x => x.DetailsForHelps)
            .Must(x => x.Count <= DetailsForHelps.MAX_DETAILS_COUNT)
            .WithMessageCustom("DetailsForHelps count can't be greater than maximum");
    }
}