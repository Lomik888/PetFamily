using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

namespace PetFamily.Volunteers.Application.Commands.UpdateStatusPet;

public class UpdateStatusPetCommandValidator : AbstractValidator<UpdateStatusPetCommand>
{
    public UpdateStatusPetCommandValidator()
    {
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
        RuleFor(x => x.HelpStatus).MustBeValueObject(x => HelpStatus.Create(x));
    }
}