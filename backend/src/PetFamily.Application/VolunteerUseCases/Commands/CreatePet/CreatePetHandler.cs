using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Extensions;
using PetFamily.Application.Repositories;
using PetFamily.Domain.Contracts;
using PetFamily.Domain.SpeciesContext.Ids;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Errors.Enums;

namespace PetFamily.Application.VolunteerUseCases.Commands.CreatePet;

public class CreatePetHandler : ICommandHandler<ErrorList, CreatePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<CreatePetCommand> _validator;
    private readonly ILogger<CreatePetHandler> _logger;
    private readonly ISqlConnectionFactory _connectionFactory;

    public CreatePetHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<CreatePetCommand> validator,
        ILogger<CreatePetHandler> logger,
        ISpeciesRepository speciesRepository,
        ISqlConnectionFactory connectionFactory)
    {
        _volunteerRepository = volunteerRepository ??
                               throw new ArgumentNullException(
                                   nameof(volunteerRepository),
                                   "Volunteer Repository is missing");

        _validator = validator ??
                     throw new ArgumentNullException(
                         nameof(validator),
                         "Create Pet Command Validator is missing");

        _logger = logger ??
                  throw new ArgumentNullException(
                      nameof(logger),
                      "Logger is missing");

        _speciesRepository = speciesRepository ??
                             throw new ArgumentNullException(
                                 nameof(speciesRepository),
                                 "Species Repository is missing");
        _connectionFactory = connectionFactory;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        CreatePetCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var volunteerId = VolunteerId.Create(request.VolunteerId).Value;
        var speciesId = SpeciesId.Create(request.SpeciesBreedIdDto.SpeciesId).Value;
        var breedId = BreedId.Create(request.SpeciesBreedIdDto.BreedId).Value;

        var volunteer = await _volunteerRepository.GetByIdWithPetsAsync(volunteerId, cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("@speciesId", request.SpeciesBreedIdDto.SpeciesId);
        parameters.Add("@breedsId", request.SpeciesBreedIdDto.BreedId);

        var sql = $"""
                   select exists(
                      select 1
                          from breeds as b
                          right join species as s on s.id = b.species_id 
                          where 
                              s.id = @speciesId 
                            and b.id = @breedsId) as result
                   """;
        using var connection = _connectionFactory.Create();

        var speciesAndBreedExistResult = await connection.QuerySingleAsync<bool>(sql, parameters);
        if (speciesAndBreedExistResult == false)
        {
            var error = Error.Create(
                "Species and Breed do not exist.",
                ErrorCodes.General.NotFound,
                ErrorType.NOTFOUND);

            return ErrorList.Create(error);
        }

        var detailsForHelpList = request.DetailsForHelps
            .Select(x => DetailsForHelp.Create(x.Title, x.Description).Value);

        var petId = PetId.Create().Value;
        var nickName = NickName.Create(request.NickName).Value;
        var speciesBreedId = SpeciesBreedId
            .Create(
                request.SpeciesBreedIdDto.SpeciesId,
                request.SpeciesBreedIdDto.BreedId)
            .Value;
        var description = Description.Create(request.Description).Value;
        var color = Color.Create(request.Color).Value;
        var healthDescription = HealthDescription
            .Create(
                request.HealthDescriptionDto.SharedHealthStatus,
                request.HealthDescriptionDto.SkinCondition,
                request.HealthDescriptionDto.MouthCondition,
                request.HealthDescriptionDto.DigestiveSystemCondition)
            .Value;
        var address = Address
            .Create(
                request.AddressDto.Country,
                request.AddressDto.City,
                request.AddressDto.Street,
                request.AddressDto.HouseNumber,
                request.AddressDto.ApartmentNumber
            )
            .Value;
        var wight = Weight.Create(request.Weight).Value;
        var height = Height.Create(request.Height).Value;
        var phoneNumber = PhoneNumber.Create(
                request.PhoneNumberDto.RegionCode,
                request.PhoneNumberDto.Number)
            .Value;
        var sterilize = request.Sterilize;
        var dateOfBirth = DateOfBirth.Create(request.DateOfBirth).Value;
        var vaccinated = request.Vaccinated;
        var helpStatus = HelpStatus.Create(request.HelpStatus).Value;
        var detailsForHelps = DetailsForHelps.Create(detailsForHelpList).Value;
        var filesPet = FilesPet.CreateEmpty().Value;

        var createPetDto = new CreatePetDto(
            nickName,
            speciesBreedId,
            description,
            color,
            healthDescription,
            address,
            wight,
            height,
            phoneNumber,
            sterilize,
            dateOfBirth,
            vaccinated,
            helpStatus,
            detailsForHelps,
            filesPet);

        var createPetResult = volunteer.CreatePet(createPetDto);
        if (createPetResult.IsFailure == true)
        {
            return ErrorList.Create(createPetResult.Error);
        }

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);
        return UnitResult.Success<ErrorList>();
    }
}