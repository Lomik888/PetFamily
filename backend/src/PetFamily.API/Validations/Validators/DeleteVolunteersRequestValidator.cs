using FluentValidation;
using PetFamily.API.Contracts.Requests.Volunteer;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteerUseCases.Delete;

namespace PetFamily.API.Validations.Validators;

public class DeleteVolunteersRequestValidator : AbstractValidator<DeleteVolunteersRequest>
{
    public DeleteVolunteersRequestValidator()
    {
        RuleFor(x => x.DeleteType)
            .Must(x => Enum.IsDefined(typeof(DeleteType), x))
            .WithMessageCustom("Invalid delete type");
    }
}