using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

namespace PetFamily.Volunteers.Application.Commands.MovePet;

public class MovePetHandler : ICommandHandler<ErrorList, MovePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<MovePetCommand> _validator;
    private readonly ILogger<MovePetHandler> _logger;

    public MovePetHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<MovePetCommand> validator,
        ILogger<MovePetHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        MovePetCommand request,
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
        var pet = volunteer.Pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
        {
            var error = ErrorsPreform.General.NotFound(petId.Value);
            return ErrorList.Create(error);
        }

        var serialNumber = SerialNumber.Create(request.SerialNumber).Value;

        volunteer.MovePet(pet, serialNumber);

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}