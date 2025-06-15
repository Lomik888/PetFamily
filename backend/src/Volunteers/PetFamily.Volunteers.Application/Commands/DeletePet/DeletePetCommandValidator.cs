using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Commands.Delete;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Commands.DeletePet;

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