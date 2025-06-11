using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(x => x.Email).MustBeValueObject(x => Email.Create(x));
        RuleFor(x => x.Description).MustBeValueObject(x => Description.Create(x));
        RuleFor(x => x.Experience).MustBeValueObject(x => Experience.Create(x));

        RuleFor(x => x.VolunteerName)
            .MustBeValueObject(x => Name.Create(x.FirstName, x.LastName, x.Surname));

        RuleFor(x => x.Phone)
            .MustBeValueObject(x => PhoneNumber.Create(x.RegionCode, x.Number));
    }
}