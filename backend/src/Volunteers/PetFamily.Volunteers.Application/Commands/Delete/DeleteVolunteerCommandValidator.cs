using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Commands.Delete;

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