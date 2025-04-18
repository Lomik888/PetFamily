﻿using FluentValidation;
using PetFamily.API.Requests.Volunteer;
using PetFamily.Application.Validations;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.API.Validations;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(x => x.Email).MustBeValueObject(x => Email.Create(x));
        RuleFor(x => x.Description).MustBeValueObject(x => Description.Create(x));
        RuleFor(x => x.Experience).MustBeValueObject(x => Experience.Create(x));

        RuleFor(x => x.Name)
            .MustBeValueObject(x => Name.Create(x.FirstName, x.LastName, x.Surname));

        RuleFor(x => x.Phone)
            .MustBeValueObject(x => PhoneNumber.Create(x.RegionCode, x.Number));
    }
}