using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteerUseCases.Commands.Delete;
using PetFamily.Domain.VolunteerContext.IdsVO;

namespace PetFamily.Application.VolunteerUseCases.Commands.DeletePet;

public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetCommandValidator()
    {
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
        RuleFor(x => x.DeleteType)
            .Must(x => Enum.IsDefined(typeof(DeleteType), x))
            .WithMessageCustom("Invalid deleteType value.");
    }
}