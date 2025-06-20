using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;

namespace PetFamily.Volunteers.Application.Commands.UpdateDetailsForHelps;

public class UpdateVolunteersDetailsForHelpHandler :
    ICommandHandler<ErrorList, UpdateVolunteersDetailsForHelpCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<UpdateVolunteersDetailsForHelpCommand> _validator;
    private readonly ILogger<UpdateVolunteersDetailsForHelpHandler> _logger;

    public UpdateVolunteersDetailsForHelpHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateVolunteersDetailsForHelpHandler> logger,
        IValidator<UpdateVolunteersDetailsForHelpCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
    }


    public async Task<UnitResult<ErrorList>> Handle(
        UpdateVolunteersDetailsForHelpCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            return ErrorList.Create(validationResult.Errors.ToErrors());
        }

        var volunteer = await _volunteerRepository.GetByIdAsync(
            VolunteerId.Create(request.VolunteerId).Value,
            cancellationToken);

        var newDetailsForHelp = request.DetailsForHelpCollection.DetailsForHelps
            .Select(x => DetailsForHelp.Create(x.Title, x.Description).Value);

        var newDetailsForHelps = DetailsForHelps.Create(newDetailsForHelp).Value;

        //volunteer.SetDetailsForHelps(newDetailsForHelps);

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        _logger.LogInformation("Updated volunteer details for helps");
        return UnitResult.Success<ErrorList>();
    }
}