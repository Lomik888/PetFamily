using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Commands.UploadPetFiles;

public class UploadPetFilesCommandValidation : AbstractValidator<UploadPetFilesCommand>
{
    public UploadPetFilesCommandValidation()
    {
        RuleFor(x => x.PetFilesDtos).NotNull().WithMessageCustom("PetFilesDtos cannot be null");

        RuleForEach(x => x.PetFilesDtos).ChildRules(x =>
        {
            x.RuleFor(x => x.FileInfoDto.Name).NotEmpty().NotNull().WithMessageCustom("Name cannot be empty");
            x.RuleFor(x => x.FileStream).NotNull().WithMessageCustom("FileStream cannot be empty");
            x.RuleFor(x => x.FileInfoDto.Extension)
                .NotEmpty().WithMessageCustom("Extension cannot be empty")
                .NotNull().WithMessageCustom("Extension cannot be null");
        });

        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
    }
}