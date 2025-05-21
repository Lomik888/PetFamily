using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Application.VolunteerUseCases.Commands.SetMainFilePet;

public class SetMainFilePetCommandValidator : AbstractValidator<SetMainFilePetCommand>
{
    public SetMainFilePetCommandValidator()
    {
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
        RuleFor(x => x.FullPath).MustBeValueObject(x => File.Create(x));
    }
}