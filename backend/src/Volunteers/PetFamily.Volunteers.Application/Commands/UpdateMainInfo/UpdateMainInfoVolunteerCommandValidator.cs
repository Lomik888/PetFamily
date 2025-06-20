using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Validation;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

public class UpdateMainInfoVolunteerCommandValidator : AbstractValidator<UpdateMainInfoVolunteerCommand>
{
    public UpdateMainInfoVolunteerCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));

        RuleFor(x => x.Description)
            .MustBeValueObject(x => Description.Create(x!))
            .When(x => string.IsNullOrWhiteSpace(x.Description));

        // RuleFor(x => x.Experience)
        //     .MustBeValueObject(x => Experience.Create((int)x!))
        //     .When(x => x.Experience != null);

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .MustBeValueObject(x =>
                    Validator.FieldValueObject.ValidationNullable(
                        x?.FirstName,
                        Name.MIN_LENGTH,
                        Name.FIRST_NAME_LENGTH))
                .When(x => string.IsNullOrWhiteSpace(x.Name?.FirstName) == false)
                .MustBeValueObject(x =>
                    Validator.FieldValueObject.ValidationNullable(
                        x?.LastName,
                        Name.MIN_LENGTH,
                        Name.FIRST_NAME_LENGTH))
                .When(x => string.IsNullOrWhiteSpace(x.Name?.LastName) == false)
                .MustBeValueObject(x =>
                    Validator.FieldValueObject.ValidationNullable(
                        x?.Surname,
                        Name.MIN_LENGTH,
                        Name.FIRST_NAME_LENGTH))
                .When(x => string.IsNullOrWhiteSpace(x.Name?.Surname) == false);
        });
    }
}