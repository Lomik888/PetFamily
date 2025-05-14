namespace PetFamily.Application.Contracts.DTO;

public record FilePathDto(
    string BucketName,
    string SubBucketName,
    Guid UserId,
    Guid PetId,
    string ObjectName,
    string Extension);