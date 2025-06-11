using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Commands.Activate;

public class ActivateVolunteerCommandValidator : AbstractValidator<ActivateVolunteerCommand>
{
    public ActivateVolunteerCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));
    }
}