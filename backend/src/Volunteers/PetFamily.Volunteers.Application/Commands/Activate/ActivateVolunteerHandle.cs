﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Commands.Activate;

public class ActivateVolunteerHandle :
    ICommandHandler<ErrorList, ActivateVolunteerCommand>
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

    public async Task<UnitResult<ErrorList>> Handle(
        ActivateVolunteerCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            return ErrorList.Create(validationResult.Errors.ToErrors());
        }

        var isActivate = false;

        var volunteer = await _volunteerRepository
            .GetByIdWithPetsAsync(
                VolunteerId.Create(request.VolunteerId).Value,
                isActivate,
                cancellationToken);

        volunteer.Activate();

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        _logger.LogInformation("Activated volunteer {0} and volunteer's pets", volunteer.Id.Value);
        return UnitResult.Success<ErrorList>();
    }
}