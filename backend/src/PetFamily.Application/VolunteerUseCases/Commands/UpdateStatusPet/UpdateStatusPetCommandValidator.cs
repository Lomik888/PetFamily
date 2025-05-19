using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Application.VolunteerUseCases.Commands.UpdateStatusPet;

public class UpdateStatusPetCommandValidator : AbstractValidator<UpdateStatusPetCommand>
{
    public UpdateStatusPetCommandValidator()
    {
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
        RuleFor(x => x.HelpStatus).MustBeValueObject(x => HelpStatus.Create(x));
    }
}