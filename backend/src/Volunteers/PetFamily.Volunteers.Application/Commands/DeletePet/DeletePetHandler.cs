using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Dtos;
using PetFamily.Core.Enums;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Errors.Enums;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Application.Commands.Delete;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using File = PetFamily.SharedKernel.ValueObjects.File;
using IsolationLevel = System.Data.IsolationLevel;

namespace PetFamily.Volunteers.Application.Commands.DeletePet;

public class DeletePetHandler : ICommandHandler<ErrorList, DeletePetCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<DeletePetCommand> _validator;
    private readonly ILogger<DeletePetHandler> _logger;
    private readonly IFilesProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChannelMessageQueue _invalidFilesMessageQueue;

    public DeletePetHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetHandler> logger,
        IValidator<DeletePetCommand> validator,
        IFilesProvider fileProvider,
        [FromKeyedServices(UnitOfWorkTypes.Volunteers)]
        IUnitOfWork unitOfWork,
        IChannelMessageQueue invalidFilesMessageQueue)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _invalidFilesMessageQueue = invalidFilesMessageQueue;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        DeletePetCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            return ErrorList.Create(validationResult.Errors.ToErrors());
        }

        var volunteerId = VolunteerId.Create(request.VolunteerId).Value;
        var petId = PetId.Create(request.PetId).Value;
        var volunteer = await _volunteerRepository.GetByIdWithPetsAsync(volunteerId, cancellationToken);
        var pet = volunteer.Pets.SingleOrDefault(x => x.Id == petId);
        if (pet is null)
        {
            _logger.LogInformation("Pet with id {Id} not found", petId);
            var error = ErrorsPreform.General.NotFound(petId.Value);
            return ErrorList.Create(error);
        }

        var result = await DeletePetAsync(volunteer, pet, request.DeleteType, cancellationToken);
        return result.IsSuccess == false ? result.Error : UnitResult.Success<ErrorList>();
    }

    private async Task<UnitResult<ErrorList>> DeletePetAsync(
        Volunteer volunteer,
        Pet pet,
        DeleteType deleteType,
        CancellationToken cancellationToken)
    {
        volunteer.UnActivatePet(pet);

        switch (deleteType)
        {
            case DeleteType.SOFT:
                await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);
                _logger.LogInformation("Pet {0} active status updated", pet.Id.Value);
                break;
            case DeleteType.HARD:
                var result = await HardDeletePetAsync(volunteer, pet, cancellationToken);
                if (result.IsSuccess == false)
                {
                    _logger.LogInformation("Pet {0} can't hard deleted", pet.Id.Value);
                    return ErrorList.Create(result.Error);
                }

                _logger.LogInformation("Pet {0} deleted", pet.Id.Value);
                break;
            default:
                return ErrorList.Create(new[]
                {
                    Error.Create("Something Wrong", null, ErrorType.EXCEPTION)
                });
        }

        return UnitResult.Success<ErrorList>();
    }

    private async Task<UnitResult<Error>> HardDeletePetAsync(
        Volunteer volunteer,
        Pet pet,
        CancellationToken cancellationToken)
    {
        volunteer.UnActivatePet(pet);
        volunteer.DeletePet(pet);
        try
        {
            var isolationLevel = IsolationLevel.ReadCommitted;
            await _unitOfWork.BeginTransactionAsync(isolationLevel, cancellationToken);

            await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);
            var deletePetFilesTask = DeletePetFilesTask(pet, cancellationToken);

            var results = await Task.WhenAll(deletePetFilesTask);

            var deletedFiles = results
                .Where(x => x.Item2.IsSuccess == true)
                .Select(x => x.Item1.FullPath).ToList();
            var failDeletedFiles = results
                .Where(x => x.Item2.IsSuccess == false)
                .Select(x => x.Item1.FullPath).ToList();

            var filesToDelete = failDeletedFiles.Select(x => FileToDeleteDto.Create(x).Value);

            await _invalidFilesMessageQueue.AddPathsAsync(filesToDelete, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, ex.Message);
            var error = ErrorsPreform.General.Unknown(ex.Message);
            return UnitResult.Failure(error);
        }
    }

    private List<Task<(File file, Result<string, Error> result)>> DeletePetFilesTask(Pet pet,
        CancellationToken cancellationToken)
    {
        return pet.FilesPet.Items.Select(async x =>
        {
            _logger.LogInformation(
                "{x.FullPath} начал удаление",
                x.FullPath);

            var result = await _fileProvider.RemoveAsync(x.FullPath, cancellationToken);

            _logger.LogInformation(
                "{x.FullPath} закончил удаление",
                x.FullPath);

            return (x, result);
        }).ToList();
    }
}