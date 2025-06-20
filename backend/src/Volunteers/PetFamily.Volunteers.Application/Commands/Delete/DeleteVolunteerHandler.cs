﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Errors.Enums;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Commands.Delete;

public class DeleteVolunteerHandler :
    ICommandHandler<ErrorList, DeleteVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHandler> logger,
        IValidator<DeleteVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeleteVolunteerCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            return ErrorList.Create(validationResult.Errors.ToErrors());
        }

        var isActivate = true;
        Volunteer volunteer;

        switch (request.DeleteType)
        {
            case DeleteType.SOFT:
                volunteer = await _volunteerRepository
                    .GetByIdWithPetsAsync(
                        VolunteerId.Create(request.VolunteerId).Value,
                        isActivate,
                        cancellationToken);
                volunteer.UnActivate();
                await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);
                _logger.LogInformation("Volunteer {0} active status updated", volunteer.Id.Value);
                break;

            case DeleteType.HARD:
                volunteer = await _volunteerRepository
                    .GetByIdAsync(
                        VolunteerId.Create(request.VolunteerId).Value,
                        isActivate,
                        cancellationToken);
                await _volunteerRepository.HardDelete(volunteer, cancellationToken);
                _logger.LogInformation("Volunteer {0} deleted", volunteer.Id.Value);
                break;

            default:
                return ErrorList.Create(new[]
                {
                    Error.Create("Something Wrong", null, ErrorType.EXCEPTION)
                });
        }

        return UnitResult.Success<ErrorList>();
    }
}