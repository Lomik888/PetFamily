using FluentValidation;
using PetFamily.API.Contracts.Requests.Volunteer;
using PetFamily.Application.VolunteerUseCases.SoftDelete;

namespace PetFamily.API.Validations.Validators;

public class UpdateVolunteersRequestValidator : AbstractValidator<DeleteVolunteersRequest>
{
    public UpdateVolunteersRequestValidator()
    {
        RuleFor(x => x.DeleteType)
            .Must(x => Enum.IsDefined(typeof(DeleteType), x))
            .WithMessage("Invalid deleteType value.");
    }
}