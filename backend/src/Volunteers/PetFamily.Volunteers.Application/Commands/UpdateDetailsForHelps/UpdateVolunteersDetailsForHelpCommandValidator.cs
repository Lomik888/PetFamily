using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO.Collections;

namespace PetFamily.Volunteers.Application.Commands.UpdateDetailsForHelps;

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