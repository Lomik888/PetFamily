using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Shared.Validation;

namespace PetFamily.Application.VolunteerUseCases.UpdateMainInfo;

public class UpdateMainInfoVolunteerCommandValidator : AbstractValidator<UpdateMainInfoVolunteerCommand>
{
    public UpdateMainInfoVolunteerCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .MustBeValueObject(x => VolunteerId.Create(x));

        RuleFor(x => x.Description)
            .MustBeValueObject(x => Description.Create(x!))
            .When(x => string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Experience)
            .MustBeValueObject(x => Experience.Create((int)x!))
            .When(x => x.Experience != null);

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .MustBeValueObject(x =>
                    FieldValidator.ValidationNullableField(
                        x?.FirstName,
                        Name.MIN_LENGTH,
                        Name.FIRST_NAME_LENGTH))
                .When(x => string.IsNullOrWhiteSpace(x.Name?.FirstName) == false)
                .MustBeValueObject(x =>
                    FieldValidator.ValidationNullableField(
                        x?.LastName,
                        Name.MIN_LENGTH,
                        Name.FIRST_NAME_LENGTH))
                .When(x => string.IsNullOrWhiteSpace(x.Name?.LastName) == false)
                .MustBeValueObject(x =>
                    FieldValidator.ValidationNullableField(
                        x?.Surname,
                        Name.MIN_LENGTH,
                        Name.FIRST_NAME_LENGTH))
                .When(x => string.IsNullOrWhiteSpace(x.Name?.Surname) == false);
        });
    }
}