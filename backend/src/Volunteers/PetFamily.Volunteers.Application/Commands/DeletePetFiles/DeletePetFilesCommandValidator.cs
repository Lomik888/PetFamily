using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Volunteers.Application.Commands.DeletePetFiles;

public class DeletePetFilesCommandValidator : AbstractValidator<DeletePetFilesCommand>
{
    public DeletePetFilesCommandValidator()
    {
        RuleForEach(x => x.FullFilePath).MustBeValueObject(x => File.Create(x));
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
    }
}