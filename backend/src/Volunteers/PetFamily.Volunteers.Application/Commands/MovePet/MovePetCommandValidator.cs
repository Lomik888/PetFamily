using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

namespace PetFamily.Volunteers.Application.Commands.MovePet;

public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
{
    public MovePetCommandValidator()
    {
        RuleFor(x => x.SerialNumber).MustBeValueObject(x => SerialNumber.Create(x));
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
    }
}