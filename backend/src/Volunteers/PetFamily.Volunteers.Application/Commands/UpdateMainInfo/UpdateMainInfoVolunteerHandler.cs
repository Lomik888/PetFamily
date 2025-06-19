using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Errors.Enums;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Application.Commands.Create;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;

namespace PetFamily.Volunteers.Application.Commands.UpdateMainInfo;

public class UpdateMainInfoVolunteerHandler : ICommandHandler<Guid, ErrorList, UpdateMainInfoVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<UpdateMainInfoVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public UpdateMainInfoVolunteerHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<CreateVolunteerHandler> logger,
        IValidator<UpdateMainInfoVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoVolunteerCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid Validation request");
            return ErrorList.Create(validationResult.Errors.ToErrors());
        }

        var volunteer = await _volunteerRepository.GetByIdAsync(
            VolunteerId.Create(request.VolunteerId).Value,
            cancellationToken);

        string? firstName = null;
        string? lastName = null;
        string? sureName = null;
        Description? description = null;
        //Experience? experience = null;

        foreach (var req in request.GetNotNullPropertiesEnumerator())
        {
            switch (req.Name)
            {
                case "Description":
                    description = Description.Create(req!.Value!.ToString()!).Value;
                    break;
                // case "Experience":
                //     experience = Experience.Create((int)req!.Value!).Value;
                //     break;
                case "FirstName":
                    firstName = req.Value!.ToString()!;
                    break;
                case "LastName":
                    lastName = req.Value!.ToString()!;
                    break;
                case "Surname":
                    sureName = req.Value!.ToString()!;
                    break;
                case "VolunteerId":
                    break;
                default:
                    return ErrorList.Create(new[]
                    {
                        Error.Create("Nothing to update", null, ErrorType.NONE)
                    });
            }
        }

        volunteer.UpdateMainInfo(
            Name.Create(
                firstName ?? volunteer.Name.FirstName,
                lastName ?? volunteer.Name.LastName,
                sureName ?? volunteer.Name.Surname
            ).Value,
            description ?? volunteer.Description
            //experience ?? volunteer.Experience
        );

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        _logger.LogInformation("Volunteer updated");
        return volunteer.Id.Value;
    }
}