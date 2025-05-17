using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;

namespace PetFamily.Application.VolunteerUseCases.Commands.CreatePet;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.NickName).MustBeValueObject(x => NickName.Create(x));

        RuleFor(x => x.SpeciesBreedIdDto)
            .MustBeValueObject(x => SpeciesBreedId.Create(x.SpeciesId, x.BreedId));

        RuleFor(x => x.Description).MustBeValueObject(x => Description.Create(x));
        RuleFor(x => x.Color).MustBeValueObject(x => Color.Create(x));

        RuleFor(x => x.HealthDescriptionDto)
            .MustBeValueObject(x => HealthDescription.Create(
                x.SharedHealthStatus,
                x.SkinCondition,
                x.MouthCondition,
                x.DigestiveSystemCondition));

        RuleFor(x => x.AddressDto)
            .MustBeValueObject(x => Address.Create(
                x.Country,
                x.City,
                x.Street,
                x.HouseNumber,
                x.ApartmentNumber));

        RuleFor(x => x.Weight).MustBeValueObject(x => Weight.Create(x));
        RuleFor(x => x.Height).MustBeValueObject(x => Height.Create(x));

        RuleFor(x => x.PhoneNumberDto)
            .MustBeValueObject(x => PhoneNumber.Create(x.RegionCode, x.Number));

        RuleFor(x => x.DateOfBirth).MustBeValueObject(x => DateOfBirth.Create(x));
        RuleFor(x => x.HelpStatus).MustBeValueObject(x => HelpStatus.Create(x));

        RuleForEach(x => x.DetailsForHelps)
            .MustBeValueObject(x => DetailsForHelp.Create(x.Title, x.Description));

        RuleFor(x => x.DetailsForHelps)
            .Must(x => x.Count() <= DetailsForHelps.MAX_DETAILS_COUNT)
            .WithMessageCustom("DetailsForHelps count can't be greater than maximum");
    }
}