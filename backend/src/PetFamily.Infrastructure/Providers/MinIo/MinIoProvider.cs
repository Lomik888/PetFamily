using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.Providers;

namespace PetFamily.Infrastructure.Providers.MinIo;

public class MinIoProvider : IFilesProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinIoProvider> _logger;

    public MinIoProvider(IMinioClient minioClient, ILogger<MinIoProvider> logger)
    {
        _minioClient = minioClient ??
                       throw new ArgumentNullException(nameof(minioClient), "MinioClient missing");
        _logger = logger ??
                  throw new ArgumentNullException(nameof(logger), "Logger missing");
    }

    public async Task<string> PresignedGetAsync(
        string bucketName,
        string userId,
        string objectName,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Generating presigned url for userId: {userId}");

        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("BucketName required", nameof(bucketName));
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId required", nameof(userId));
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("ObjectName required", nameof(objectName));

        _logger.LogInformation($"Validation successes for userId: {userId}");

        var fullObjectKey = $"{userId}/{objectName}";

        var pesignedGetObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fullObjectKey)
            .WithExpiry(1000);

        var response = await _minioClient.PresignedGetObjectAsync(pesignedGetObjectArgs)
            .ConfigureAwait(false);

        _logger.LogInformation($"Url Created file for userId: {userId}");

        return response;
    }

    public async Task RemoveAsync(
        string bucketName,
        string userId,
        string objectName,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Removing for userId: {objectName}");

        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("BucketName required", nameof(bucketName));
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId required", nameof(userId));
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("ObjectName required", nameof(objectName));

        _logger.LogInformation($"Validation successes for userId: {userId}");

        var fullObjectKey = $"{userId}/{objectName}";

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fullObjectKey);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation($"Removed file for userId: {userId}");
    }

    public async Task UploadAsync(
        string bucketName,
        string userId,
        string objectName,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Uploading for userId: {userId}");

        if (string.IsNullOrWhiteSpace(bucketName))
            throw new ArgumentException("BucketName required", nameof(bucketName));
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId required", nameof(userId));
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("ObjectName required", nameof(objectName));
        if (stream is null)
            throw new ArgumentNullException(nameof(stream), "Stream can't be null");

        _logger.LogInformation($"Validation successes for userId: {userId}");

        await EnsureBucketExistsAsync(bucketName, cancellationToken).ConfigureAwait(false);

        var fullObjectKey = $"{userId}/{objectName}";

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fullObjectKey)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation($"Uploaded file for userId: {userId}");
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