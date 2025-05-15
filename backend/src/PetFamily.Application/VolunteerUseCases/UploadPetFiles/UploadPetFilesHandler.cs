using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Contracts.DTO;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Application.Repositories;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Shared.Errors;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Application.VolunteerUseCases.UploadPetFiles;

public class UploadPetFilesHandler : ICommandHandler<ErrorList, UploadPetFilesCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<UploadPetFilesCommand> _validator;
    private readonly ILogger<UploadPetFilesHandler> _logger;
    private readonly IFilesProvider _filesProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UploadPetFilesHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UploadPetFilesHandler> logger,
        IValidator<UploadPetFilesCommand> validator,
        IFilesProvider filesProvider,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _filesProvider = filesProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        UploadPetFilesCommand request,
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

        var uploadedFiles = new List<string>();
        var failUploadedFiles = new List<Error>();

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

            var results = await UploadFilesAsync(request, cancellationToken);

            _logger.LogInformation("{uploadedFiles.Count} файлов было загружено ", uploadedFiles.Count);
            _logger.LogInformation("{failUploadedFiles.Count} файлов не было загружено ", failUploadedFiles.Count);

            await UploadPathsAsync(uploadedFiles, failUploadedFiles, results, volunteer, pet, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Ошибка при загрузке и сохранении файлов для волонтёра {VolunteerId}, питомца {PetId}",
                volunteerId,
                petId);

            await _unitOfWork.RollbackAsync(cancellationToken);

            // отправить пути файлов, которые надо удалить для backgroudworker
        }

        if (failUploadedFiles.Count > 0)
        {
            return ErrorList.Create(failUploadedFiles);
        }

        return UnitResult.Success<ErrorList>();
    }

    private async Task UploadPathsAsync(
        List<string> uploadedFiles,
        List<Error> failUploadedFiles,
        List<Result<string, Error>> results,
        Volunteer volunteer,
        Pet pet,
        CancellationToken cancellationToken)
    {
        uploadedFiles = results
            .Where(x => x.IsSuccess == true)
            .Select(x => x.Value)
            .ToList();
        failUploadedFiles = results
            .Where(x => x.IsFailure == true)
            .Select(x => x.Error)
            .ToList();

        var files = new List<File>();
        foreach (var file in uploadedFiles)
        {
            var fileResult = File.Create(file);
            if (fileResult.IsSuccess == false)
            {
                failUploadedFiles.Add(fileResult.Error);
            }

            files.Add(fileResult.Value);
        }

        var petFilesResult = FilesPet.Create(files);
        if (petFilesResult.IsSuccess == false)
        {
            failUploadedFiles.Add(petFilesResult.Error);
        }

        volunteer.SetPetFiles(pet, petFilesResult.Value);
        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);
    }

    private async Task<List<Result<string, Error>>> UploadFilesAsync(
        UploadPetFilesCommand request,
        CancellationToken cancellationToken)
    {
        var uploadFilesTasks = request.PetFilesDtos.Select(async x =>
        {
            _logger.LogInformation(
                "{x.Name}{x.Extension} начал загрузку",
                x.FileInfoDto.Name,
                x.FileInfoDto.Extension);

            if (x.FileInfoDto.Size > 6291456 || File.VALIDEXTENSIONS.Contains(x.FileInfoDto.Extension) == false)
            {
                return ErrorsPreform.General
                    .Unknown(
                        $"Can't upload file{x.FileInfoDto.Name}. " +
                        $"File size is {x.FileInfoDto.Size} > 6мб." +
                        $"Or File extension is {x.FileInfoDto.Extension}.");
            }

            var filePathDto = new FilePathDto(
                "volunteers",
                "pets",
                request.VolunteerId,
                request.PetId,
                x.FileInfoDto.Name,
                x.FileInfoDto.Extension);

            var result = await _filesProvider.UploadAsync(
                filePathDto,
                x.FileStream,
                cancellationToken);

            _logger.LogInformation(
                "{x.Name}{x.Extension} завершил загрузку",
                x.FileInfoDto.Name,
                x.FileInfoDto.Extension);

            return result;
        }).ToList();

        var results = await Task.WhenAll(uploadFilesTasks);
        return results.ToList();
    }
}