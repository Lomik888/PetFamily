using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Application.VolunteerUseCases.Commands.DeletePetFiles;

public class DeletePetFilesCommandValidator : AbstractValidator<DeletePetFilesCommand>
{
    public DeletePetFilesCommandValidator()
    {
        RuleForEach(x => x.FullFilePath).MustBeValueObject(x => File.Create(x));
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
    }
}