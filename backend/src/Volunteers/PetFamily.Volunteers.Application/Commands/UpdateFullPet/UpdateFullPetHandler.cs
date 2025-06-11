using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Errors.Enums;
using PetFamily.Specieses.Contracts;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.Dtos;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Collections;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Enums;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO.Collections;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using File = PetFamily.Volunteers.Domain.ValueObjects.SharedVO.File;

namespace PetFamily.Volunteers.Application.Commands.UpdateFullPet;

public class UpdateFullPetHandler : ICommandHandler<ErrorList, UpdateFullPetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesExistenceContracts _speciesExistenceContracts;
    private readonly IValidator<UpdateFullPetCommand> _validator;
    private readonly ILogger<UpdateFullPetHandler> _logger;

    public UpdateFullPetHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<UpdateFullPetCommand> validator,
        ILogger<UpdateFullPetHandler> logger,
        ISpeciesExistenceContracts speciesExistenceContracts)
    {
        _volunteerRepository = volunteerRepository;
        _validator = validator;
        _logger = logger;
        _speciesExistenceContracts = speciesExistenceContracts;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        UpdateFullPetCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        _logger.LogInformation("{UpdateFullPetHandler} validation success", nameof(UpdateFullPetHandler));

        var volunteerId = VolunteerId.Create(request.VolunteerId).Value;
        var petId = PetId.Create(request.PetId).Value;

        var volunteer = await _volunteerRepository.GetByIdWithPetsAsync(volunteerId, cancellationToken);
        var pet = volunteer.Pets.FirstOrDefault(x => x.Id == petId);
        if (pet == null)
        {
            var error = ErrorsPreform.General.NotFound(petId.Value);
            return ErrorList.Create(error);
        }

        var speciesId = request.SpeciesBreedId?.SpeciesId ?? pet.SpeciesBreedId.SpeciesId;
        var breedId = request.SpeciesBreedId?.BreedId ?? pet.SpeciesBreedId.BreedId;

        var speciesAndBreedExistsResult =
            (request.SpeciesBreedId?.SpeciesId != null || request.SpeciesBreedId?.BreedId != null) == true
                ? await SpeciesAndBreedExistsAsync(
                    speciesId,
                    breedId,
                    cancellationToken)
                : UnitResult.Success<ErrorList>();
        if (speciesAndBreedExistsResult.IsSuccess == false)
        {
            return speciesAndBreedExistsResult.Error;
        }

        _logger.LogInformation("Specie and Breed exists");

        var dto = CreateUpdatePetFullInfoDto(speciesId, breedId, pet, request);

        var result = volunteer.UpdateFullInfoAboutPet(pet, dto);
        if (result.IsSuccess == false)
        {
            return ErrorList.Create(result.Error);
        }

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);
        _logger.LogInformation("Volunteer updated");

        return UnitResult.Success<ErrorList>();
    }

    private UpdatePetFullInfoDto CreateUpdatePetFullInfoDto(
        Guid speciesId,
        Guid breedId,
        Pet pet,
        UpdateFullPetCommand request)
    {
        var nickName = request.NickName is null ? pet.NickName : NickName.Create(request.NickName).Value;
        var speciesBreedId = SpeciesBreedId.Create(speciesId, breedId).Value;
        var description = request.Description is null ? pet.Description : Description.Create(request.Description).Value;
        var color = request.Color is null ? pet.Color : Color.Create(request.Color).Value;

        var healthDescription = request.HealthDescriptionDto is null
            ? pet.HealthDescription
            : HealthDescription.Create(
                    request.HealthDescriptionDto?.SharedHealthStatus ?? pet.HealthDescription.SharedHealthStatus,
                    request.HealthDescriptionDto?.SkinCondition ?? pet.HealthDescription.SkinCondition,
                    request.HealthDescriptionDto?.MouthCondition ?? pet.HealthDescription.MouthCondition,
                    request.HealthDescriptionDto?.DigestiveSystemCondition ??
                    pet.HealthDescription.DigestiveSystemCondition)
                .Value;

        var address = request.AddressDto is null
            ? pet.Address
            : Address.Create(
                request.AddressDto.Country,
                request.AddressDto.City,
                request.AddressDto.Street,
                request.AddressDto.HouseNumber,
                request.AddressDto.ApartmentNumber
            ).Value;

        var weight = request.Weight is null ? pet.Weight : Weight.Create((double)request.Weight).Value;
        var height = request.Height is null ? pet.Height : Height.Create((double)request.Height).Value;

        var phoneNumber = request.PhoneNumberDto is null
            ? pet.PhoneNumber
            : PhoneNumber.Create(
                request.PhoneNumberDto.RegionCode,
                request.PhoneNumberDto.Number
            ).Value;
        var sterilize = request.Sterilize ?? pet.Sterilize;

        var dateOfBirth = request.DateOfBirth is null
            ? pet.DateOfBirth
            : DateOfBirth.Create((DateTime)request.DateOfBirth).Value;

        var vaccinated = request.Vaccinated ?? pet.Vaccinated;
        var helpStatus = request.HelpStatus is null
            ? pet.HelpStatus
            : HelpStatus.Create((HelpStatuses)request.Height!).Value;

        var detailsForHelps = request.DetailsForHelps is not null
            ? DetailsForHelps.Create(request.DetailsForHelps.DetailsForHelps!
                .Select(dto => DetailsForHelp.Create(dto.Title, dto.Description).Value)).Value
            : pet.DetailsForHelps;

        var filesPet = request.FilesPetDto is not null
            ? FilesPet.Create(request.FilesPetDto.FileDtos!
                .Select(dto => File.Create(dto.Path).Value)).Value
            : pet.FilesPet;

        var dto = new UpdatePetFullInfoDto(
            nickName,
            speciesBreedId,
            description,
            color,
            healthDescription,
            address,
            weight,
            height,
            phoneNumber,
            sterilize,
            dateOfBirth,
            vaccinated,
            helpStatus,
            detailsForHelps,
            filesPet
        );

        return dto;
    }

    private async Task<UnitResult<ErrorList>> SpeciesAndBreedExistsAsync(
        Guid speciesId,
        Guid breedId,
        CancellationToken cancellationToken)
    {
        var result = await _speciesExistenceContracts.SpeciesAndBreedExistsAsync(speciesId, breedId, cancellationToken);
        if (result.IsSuccess == false)
        {
            return result.Error;
        }

        if (result.Value == false)
        {
            var error = Error.Create(
                "Species and Breed do not exist.",
                ErrorCodes.General.NotFound,
                ErrorType.NOTFOUND);

            return ErrorList.Create(error);
        }

        return UnitResult.Success<ErrorList>();
    }
}