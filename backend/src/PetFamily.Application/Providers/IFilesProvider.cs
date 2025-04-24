namespace PetFamily.Application.Providers;

public interface IFilesProvider
{
    Task UploadAsync(
        string bucketName,
        string userId,
        string objectName,
        Stream stream,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(
        string bucketName,
        string userId,
        string objectName,
        CancellationToken cancellationToken = default);

    Task<string> PresignedGetAsync(
        string bucketName,
        string userId,
        string objectName,
        CancellationToken cancellationToken = default);
}