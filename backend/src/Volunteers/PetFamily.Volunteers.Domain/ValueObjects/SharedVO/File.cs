using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Volunteers.Domain.ValueObjects.SharedVO;

public class File : ValueObject
{
    public static List<string> VALIDEXTENSIONS = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mp3" };

    public string FullPath { get; }

    [JsonConstructor]
    private File(string fullPath)
    {
        FullPath = fullPath;
    }

    public static Result<File, Error> Create(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            return ErrorsPreform.General.Validation("File path is required", nameof(fullPath));

        if (VALIDEXTENSIONS.Contains(Path.GetExtension(fullPath)) == false)
            return ErrorsPreform.General.Validation("File extension is invalid", nameof(fullPath));

        return new File(fullPath);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FullPath;
    }
}