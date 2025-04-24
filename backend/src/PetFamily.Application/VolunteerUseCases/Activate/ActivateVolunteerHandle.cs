using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteerUseCases.Delete;
using PetFamily.Application.VolunteerUseCases.SoftDelete;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Errors.Enums;

namespace PetFamily.Application.VolunteerUseCases.Activate;

public class ActivateVolunteerHandle :
    ICommandHandler<ErrorCollection, ActivateVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<ActivateVolunteerCommand> _validator;
    private readonly ILogger<ActivateVolunteerHandle> _logger;

    public ActivateVolunteerHandle(
        IVolunteerRepository volunteerRepository,
        ILogger<ActivateVolunteerHandle> logger,
        IValidator<ActivateVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorCollection>> Handle(
        ActivateVolunteerCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            return ErrorCollection.Create(validationResult.Errors.ToErrors());
        }

        var isActivate = false;

        var volunteer = await _volunteerRepository
            .GetByIdWitchPetsAsync(
                VolunteerId.Create(request.VolunteerId).Value,
                isActivate,
                cancellationToken);

        volunteer.Activate();

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        _logger.LogInformation("Activated volunteer {0} and volunteer's pets", volunteer.Id.Value);
        return UnitResult.Success<ErrorCollection>();
    }
}