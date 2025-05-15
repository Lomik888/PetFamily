using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.Contracts;

public class FileToDelete
{
    public string Path { get; private set; }
    public uint CountRetry { get; private set; }

    public FileToDelete(
        string path,
        uint countRetry)
    {
        Path = path;
        CountRetry = countRetry;
    }

    public static Result<FileToDelete, Error> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return ErrorsPreform.General.Validation("Path is required", path);

        return new FileToDelete(path, 0);
    }

    public void PlusCountRetry()
    {
        CountRetry++;
    }
}