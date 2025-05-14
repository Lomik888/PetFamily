using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.VolunteerUseCases.MovePet;

public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
{
    public MovePetCommandValidator()
    {
        RuleFor(x => x.SerialNumber).MustBeValueObject(x => SerialNumber.Create(x));
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
    }
}