using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;

namespace PetFamily.Application.VolunteerUseCases.Commands.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));

        RuleFor(x => x.DeleteType)
            .Must(x => Enum.IsDefined(typeof(DeleteType), x))
            .WithMessageCustom("Invalid deleteType value.");
    }
}