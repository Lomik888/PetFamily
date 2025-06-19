using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Validation;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Collections;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Volunteers.Application.Commands.UpdateFullPet;

public class UpdateFullPetCommandValidator : AbstractValidator<UpdateFullPetCommand>
{
    public UpdateFullPetCommandValidator()
    {
        RuleFor(x => x.VolunteerId).MustBeValueObject(x => VolunteerId.Create(x));
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
        When(x => x.NickName is not null, () =>
        {
            RuleFor(x => x.NickName)
                .MustBeValueObject(x => NickName.Create(x!));
        });
        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .MustBeValueObject(x => Description.Create(x!));
        });
        When(x => x.Color is not null, () =>
        {
            RuleFor(x => x.Color)
                .MustBeValueObject(x => Color.Create(x!));
        });
        When(x => x.Weight is not null, () =>
        {
            RuleFor(x => x.Weight)
                .MustBeValueObject(x => Weight.Create((double)x!));
        });
        When(x => x.Height is not null, () =>
        {
            RuleFor(x => x.Height)
                .MustBeValueObject(x => Height.Create((double)x!));
        });
        When(x => x.DateOfBirth is not null, () =>
        {
            RuleFor(x => x.DateOfBirth)
                .MustBeValueObject(x => DateOfBirth.Create((DateTime)x!));
        });
        When(x => x.HelpStatus is not null, () =>
        {
            RuleFor(x => x.HelpStatus)
                .MustBeValueObject(x => HelpStatus.Create((HelpStatuses)x!));
        });
        When(x => x.SpeciesBreedId is not null, () =>
        {
            When(x => x.SpeciesBreedId!.SpeciesId is not null, () =>
            {
                RuleFor(x => x.SpeciesBreedId!.SpeciesId)
                    .Must(x => x != Guid.Empty)
                    .WithMessageCustom("Species Id can't be empty");
            });
            When(x => x.SpeciesBreedId!.BreedId is not null, () =>
            {
                RuleFor(x => x.SpeciesBreedId!.BreedId)
                    .Must(x => x != Guid.Empty)
                    .WithMessageCustom("Breed Id can't be empty");
            });
        });
        When(x => x.HealthDescriptionDto is not null, () =>
        {
            When(x => x.HealthDescriptionDto!.SharedHealthStatus is not null, () =>
            {
                RuleFor(x => x.HealthDescriptionDto!.SharedHealthStatus)
                    .Must(x => Validator.FieldValueObject.Validation(
                        x!,
                        HealthDescription.MIN_LENGHT,
                        HealthDescription.SHAREDHEALTHSTATUS_MAX_LENGHT));
            });
            When(x => x.HealthDescriptionDto!.SkinCondition is not null, () =>
            {
                RuleFor(x => x.HealthDescriptionDto!.SkinCondition)
                    .Must(x => Validator.FieldValueObject.Validation(
                        x!,
                        HealthDescription.MIN_LENGHT,
                        HealthDescription.SKINCONDITION_MAX_LENGHT));
            });
            When(x => x.HealthDescriptionDto!.MouthCondition is not null, () =>
            {
                RuleFor(x => x.HealthDescriptionDto!.MouthCondition)
                    .Must(x => Validator.FieldValueObject.Validation(
                        x!,
                        HealthDescription.MIN_LENGHT,
                        HealthDescription.MOUTHCONDITION_MAX_LENGHT));
            });
            When(x => x.HealthDescriptionDto!.DigestiveSystemCondition is not null, () =>
            {
                RuleFor(x => x.HealthDescriptionDto!.DigestiveSystemCondition)
                    .Must(x => Validator.FieldValueObject.Validation(
                        x!,
                        HealthDescription.MIN_LENGHT,
                        HealthDescription.DIGESTIVESYSTEMCONDITION_MAX_LENGHT));
            });
        });
        When(x => x.AddressDto is not null, () =>
        {
            RuleFor(x => x.AddressDto!)
                .MustBeValueObject(x => Address.Create(
                    x.Country,
                    x.City,
                    x.Street,
                    x.HouseNumber,
                    x.ApartmentNumber));
        });
        When(x => x.PhoneNumberDto is not null, () =>
        {
            RuleFor(x => x.PhoneNumberDto!)
                .MustBeValueObject(x => PhoneNumber.Create(x.RegionCode, x.Number));
        });
        When(x => x.DetailsForHelps is not null, () =>
        {
            RuleFor(x => x.DetailsForHelps!.DetailsForHelps)
                .Must(x => x.Count <= DetailsForHelps.MAX_DETAILS_COUNT)
                .WithMessageCustom("invalid count of DetailsForHelps");

            RuleForEach(x => x.DetailsForHelps!.DetailsForHelps)
                .MustBeValueObject(x => DetailsForHelp.Create(x.Title, x.Description));
        });
        When(x => x.FilesPetDto is not null, () =>
        {
            RuleFor(x => x.FilesPetDto!.FileDtos)
                .Must(x => x.Count <= FilesPet.MAX_FILE_COUNT)
                .WithMessageCustom("invalid count of FilesPet");

            RuleForEach(x => x.FilesPetDto!.FileDtos)
                .MustBeValueObject(x => File.Create(x.Path));
        });
    }
}