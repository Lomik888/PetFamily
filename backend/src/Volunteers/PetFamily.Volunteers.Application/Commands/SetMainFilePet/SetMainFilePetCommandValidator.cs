using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Volunteers.Application.Commands.SetMainFilePet;

public class SetMainFilePetCommandValidator : AbstractValidator<SetMainFilePetCommand>
{
    public SetMainFilePetCommandValidator()
    {
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
        RuleFor(x => x.FullPath).MustBeValueObject(x => File.Create(x));
    }
}