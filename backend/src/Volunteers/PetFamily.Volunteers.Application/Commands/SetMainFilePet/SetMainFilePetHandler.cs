using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using File = PetFamily.Volunteers.Domain.ValueObjects.SharedVO.File;

namespace PetFamily.Volunteers.Application.Commands.SetMainFilePet;

public class SetMainFilePetHandler : ICommandHandler<ErrorList, SetMainFilePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<SetMainFilePetCommand> _validator;
    private readonly ILogger<SetMainFilePetHandler> _logger;

    public SetMainFilePetHandler(
        IVolunteerRepository volunteerRepository,
        IValidator<SetMainFilePetCommand> validator,
        ILogger<SetMainFilePetHandler> logger)
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
        SetMainFilePetCommand request,
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

        var file = File.Create(request.FullPath).Value;

        var result = volunteer.SetMainFilePet(pet, file);
        if (result.IsSuccess == false)
        {
            return ErrorList.Create(result.Error);
        }

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}