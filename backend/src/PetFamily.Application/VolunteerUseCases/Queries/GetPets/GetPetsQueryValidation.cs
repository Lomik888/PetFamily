using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.SpeciesContext.Ids;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Shared.Validation;

namespace PetFamily.Application.VolunteerUseCases.Queries.GetPets;

public class GetPetsQueryValidation : AbstractValidator<GetPetsQuery>
{
    public GetPetsQueryValidation()
    {
        RuleFor(x => x.Page).Must(x => x > 0).WithMessageCustom("Page must be greater than 0");
        RuleFor(x => x.PageSize).Must(x => x > -1).WithMessageCustom("PageSize must be greater than -1");
        When(x => x.VolunteerId != null,
            () =>
            {
                RuleFor(x => x.VolunteerId)
                    .MustBeValueObject(x => VolunteerId.Create((Guid)x!));
            });
        When(x => x.NickName != null,
            () =>
            {
                RuleFor(x => x.NickName)
                    .MustBeValueObject(x => NickName.Create((string)x!));
            });
        When(x => x.Color != null,
            () =>
            {
                RuleFor(x => x.Color)
                    .MustBeValueObject(x => Color.Create((string)x!));
            });
        When(x => x.Age != null,
            () =>
            {
                RuleFor(x => x.Age)
                    .MustBeValueObject(x => Age.Create((uint)x!));
            });
        When(x => x.BreedId != null,
            () =>
            {
                RuleFor(x => x.BreedId)
                    .MustBeValueObject(x => BreedId.Create((Guid)x!));
            });
        When(x => x.SpeciesId != null,
            () =>
            {
                RuleFor(x => x.SpeciesId)
                    .MustBeValueObject(x => SpeciesId.Create((Guid)x!));
            });
        When(x => x.Country != null,
            () =>
            {
                RuleFor(x => x.Country)
                    .Must(x =>
                        Validator.FieldValueObject.Validation(x!, Address.MIN_LENGHT, Address.COUNTRY_MAX_LENGHT)
                    );
            });
        When(x => x.City != null,
            () =>
            {
                RuleFor(x => x.City)
                    .Must(x =>
                        Validator.FieldValueObject.Validation(x!, Address.MIN_LENGHT, Address.CITY_MAX_LENGHT)
                    );
            });
    }
}