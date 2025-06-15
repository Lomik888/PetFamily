namespace PetFamily.Core.Dtos;

public record FilePathDto(
    string BucketName,
    string SubBucketName,
    Guid UserId,
    Guid PetId,
    string ObjectName,
    string Extension);