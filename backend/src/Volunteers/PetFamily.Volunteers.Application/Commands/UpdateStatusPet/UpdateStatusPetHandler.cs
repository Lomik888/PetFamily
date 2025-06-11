using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

namespace PetFamily.Volunteers.Application.Commands.UpdateStatusPet;

public class UpdateStatusPetHandler : ICommandHandler<ErrorList, UpdateStatusPetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<UpdateStatusPetCommand> _validator;
    private readonly ILogger<UpdateStatusPetHandler> _logger;

    public UpdateStatusPetHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<UpdateStatusPetCommand> validator,
        ILogger<UpdateStatusPetHandler> logger)
    {
        _volunteerRepository = volunteerRepository ??
                               throw new ArgumentNullException(
                                   nameof(volunteerRepository), "VolunteerRepository is missing");
        _validator = validator ??
                     throw new ArgumentNullException(
                         nameof(validator), "validator is missing");
        _logger = logger ??
                  throw new ArgumentNullException(
                      nameof(logger), "logger is missing");
    }

    public async Task<UnitResult<ErrorList>> Handle(
        UpdateStatusPetCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var volunteerId = VolunteerId.Create(request.VolunteerId).Value;
        var petId = PetId.Create(request.PetId).Value;

        var volunteer = await _volunteerRepository.GetByIdWithPetsAsync(volunteerId, cancellationToken);
        var pet = volunteer.Pets.FirstOrDefault(x => x.Id == petId);
        if (pet == null)
        {
            var error = ErrorsPreform.General.NotFound(petId.Value);
            return ErrorList.Create(error);
        }

        var newHelpStatus = HelpStatus.Create(request.HelpStatus).Value;

        var result = volunteer.SetPetStatus(pet, newHelpStatus);
        if (result.IsSuccess == false)
        {
            return ErrorList.Create(result.Error);
        }

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}