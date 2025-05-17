using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;

namespace PetFamily.Application.VolunteerUseCases.Commands.Activate;

public class ActivateVolunteerCommandValidator : AbstractValidator<ActivateVolunteerCommand>
{
    public ActivateVolunteerCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));
    }
}