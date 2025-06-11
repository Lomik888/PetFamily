using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Core.Dtos;

public class FileToDeleteDto
{
    public string Path { get; private set; }
    public uint CountRetry { get; private set; }

    public FileToDeleteDto(
        string path,
        uint countRetry)
    {
        Path = path;
        CountRetry = countRetry;
    }

    public static Result<FileToDeleteDto, Error> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return ErrorsPreform.General.Validation("Path is required", path);

        return new FileToDeleteDto(path, 0);
    }

    public void PlusCountRetry()
    {
        CountRetry++;
    }
}