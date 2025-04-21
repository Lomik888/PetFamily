using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

namespace PetFamily.Application.VolunteerUseCases.UpdateDetailsForHelps;

public class UpdateVolunteersDetailsForHelpCommandValidator : AbstractValidator<UpdateVolunteersDetailsForHelpCommand>
{
    public UpdateVolunteersDetailsForHelpCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));

        RuleForEach(x => x.DetailsForHelpCollection.DetailsForHelps)
            .MustBeValueObject(x => DetailsForHelp.Create(x.Title, x.Description));

        RuleFor(x => x.DetailsForHelpCollection.DetailsForHelps)
            .Must(x => x.Count <= DetailsForHelps.MAX_DETAILS_COUNT)
            .WithMessageCustom("DetailsForHelps count can't be greater than maximum");
    }
}