using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application;
using PetFamily.Application.Providers;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Infrastructure.Providers.MinIo;

public class MinIoProvider : IFilesProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinIoProvider> _logger;
    private readonly IMinIoLimiter _minioSemaphore;

    public MinIoProvider(
        IMinioClient minioClient,
        ILogger<MinIoProvider> logger,
        IMinIoLimiter minioSemaphore)
    {
        Validator.Guard.NotNull(minioClient);
        Validator.Guard.NotNull(logger);

        _minioClient = minioClient;
        _logger = logger;
        _minioSemaphore = minioSemaphore;
    }

    public async Task<Result<string, Error>> PredefinedGetAsync(
        string bucketName,
        string userId,
        string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Generating presigned url for userId: {userId}");

            if (string.IsNullOrWhiteSpace(bucketName))
                return ErrorsPreform.General.Validation("BucketName required", nameof(bucketName));
            if (string.IsNullOrWhiteSpace(userId))
                return ErrorsPreform.General.Validation("UserId required", nameof(userId));
            if (string.IsNullOrWhiteSpace(objectName))
                return ErrorsPreform.General.Validation("ObjectName required", nameof(objectName));

            _logger.LogInformation($"Validation successes for userId: {userId}");

            var fullObjectKey = $"{userId}/{objectName}";

            var pesignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fullObjectKey)
                .WithExpiry(1000);

            await _minioSemaphore.SemaphoreSlim.WaitAsync(cancellationToken);

            var response = await _minioClient.PresignedGetObjectAsync(pesignedGetObjectArgs)
                .ConfigureAwait(false);

            _logger.LogInformation($"Url Created file for userId: {userId}");

            return response;
        }
        catch (Exception ex)
        {
            return ErrorsPreform.General.Unknown($"Can't get file{objectName} {ex.Message}");
        }
        finally
        {
            _minioSemaphore.SemaphoreSlim.Release();
        }
    }

    public async Task<Result<string, Error>> RemoveAsync(
        string fullPath,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Removing for path: {fullPath}");

            if (string.IsNullOrWhiteSpace(fullPath))
                return ErrorsPreform.General.Validation("fullPath required", nameof(fullPath));

            _logger.LogInformation($"Validation successes for path: {fullPath}");

            var removeObjectArgs = new RemoveObjectArgs()
                .WithObject(fullPath);

            await _minioSemaphore.SemaphoreSlim.WaitAsync(cancellationToken);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation($"Removed file for path: {fullPath}");
            return fullPath;
        }
        catch (Exception ex)
        {
            return ErrorsPreform.General.Unknown($"Can't remove file{fullPath} {ex.Message}");
        }
        finally
        {
            _minioSemaphore.SemaphoreSlim.Release();
        }
    }

    public async Task<Result<string, Error>> UploadAsync(
        string bucketName,
        string subBucketName,
        Guid userId,
        Guid petId,
        string objectName,
        string extension,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Uploading for userId: {userId}");

            if (string.IsNullOrWhiteSpace(bucketName))
                return ErrorsPreform.General.Validation("BucketName required", nameof(bucketName));
            if (Guid.Empty == userId)
                return ErrorsPreform.General.Validation("UserId required", nameof(userId));
            if (Guid.Empty == petId)
                return ErrorsPreform.General.Validation("PetId required", nameof(petId));
            if (string.IsNullOrWhiteSpace(objectName))
                return ErrorsPreform.General.Validation("ObjectName required", nameof(objectName));
            if (string.IsNullOrWhiteSpace(extension))
                return ErrorsPreform.General.Validation("Extension required", nameof(extension));
            if (stream is null)
                return ErrorsPreform.General.Validation(nameof(stream), "Stream can't be null");

            _logger.LogInformation($"Validation successes for userId: {userId}");

            await _minioSemaphore.SemaphoreSlim.WaitAsync(cancellationToken);

            await EnsureBucketExistsAsync(bucketName, cancellationToken).ConfigureAwait(false);

            var objectGuid = Guid.NewGuid();

            var fullObjectKey = $"{userId}/{subBucketName}/{petId}/{objectName}_{objectGuid}{extension}";

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fullObjectKey)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length);

            var response = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation($"Uploaded file for userId: {userId}");
            return response.ObjectName;
        }
        catch (Exception ex)
        {
            return ErrorsPreform.General.Unknown($"Can't upload file{objectName} {ex.Message}");
        }
        finally
        {
            _minioSemaphore.SemaphoreSlim.Release();
        }
    }

    private async Task<bool> BucketExistsAsync(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Searching for bucketName: {bucketName}");
        var existsArgs = new BucketExistsArgs().WithBucket(bucketName);
        return await _minioClient.BucketExistsAsync(existsArgs, cancellationToken).ConfigureAwait(false);
    }

    private async Task EnsureBucketExistsAsync(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var result = await BucketExistsAsync(bucketName, cancellationToken).ConfigureAwait(false);

        if (result == false)
        {
            _logger.LogInformation($"Making new bucket {bucketName}");
            var makeArgs = new MakeBucketArgs().WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeArgs, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation($"New bucket {bucketName} generated");
        }
    }
}