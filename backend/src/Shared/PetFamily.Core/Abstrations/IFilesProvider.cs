using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Core.Abstrations;

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