using CSharpFunctionalExtensions;
using PetFamily.Application.Contracts.DTO;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.Providers;

public interface IFilesProvider
{
    Task<Result<string, Error>> UploadAsync(
        FilePathDto filePathDto,
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

