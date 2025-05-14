using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.Providers;

public interface IFilesProvider
{
    Task<Result<string, Error>> UploadAsync(
        string bucketName,
        string subBucketName,
        Guid userId,
        Guid petId,
        string objectName,
        string extension,
        Stream stream,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> RemoveAsync(
        string fullPath,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> PredefinedGetAsync(
        string bucketName,
        string userId,
        string objectName,
        CancellationToken cancellationToken = default);
}