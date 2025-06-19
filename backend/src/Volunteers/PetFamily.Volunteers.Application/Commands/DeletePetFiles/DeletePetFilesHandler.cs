using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Enums;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO.Collections;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Volunteers.Application.Commands.DeletePetFiles;

public class DeletePetFilesHandler :
    ICommandHandler<ErrorList, DeletePetFilesCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IFilesProvider _filesProvider;
    private readonly IValidator<DeletePetFilesCommand> _validator;
    private readonly ILogger<DeletePetFilesHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePetFilesHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetFilesHandler> logger,
        IValidator<DeletePetFilesCommand> validator,
        [FromKeyedServices(UnitOfWorkTypes.Volunteers)]
        IUnitOfWork unitOfWork,
        IFilesProvider filesProvider)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _filesProvider = filesProvider;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeletePetFilesCommand request,
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

        var deletedFiles = new List<string>();
        var failDeletedFiles = new List<Error>();

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

            var deletedFilesTasks = request.FullFilePath.Select(async x =>
            {
                var result = await _filesProvider.RemoveAsync(x, cancellationToken);
                return result;
            }).ToList();

            var results = await Task.WhenAll(deletedFilesTasks);

            deletedFiles = results
                .Where(x => x.IsSuccess == true)
                .Select(x => x.Value).ToList();
            failDeletedFiles = results
                .Where(x => x.IsSuccess == false)
                .Select(x => x.Error).ToList();

            if (deletedFiles.Count == 0 && failDeletedFiles.Count > 0)
            {
                return ErrorList.Create(failDeletedFiles);
            }

            var files = FilesPet.Create(deletedFiles.Select(x => File.Create(x).Value)).Value;

            volunteer.RemovePetFiles(pet, files);

            await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during the removing {ex.Message}", ex.Message);
            await _unitOfWork.RollbackAsync(cancellationToken);
        }

        if (failDeletedFiles.Count > 0)
        {
            return ErrorList.Create(failDeletedFiles);
        }

        return UnitResult.Success<ErrorList>();
    }
}