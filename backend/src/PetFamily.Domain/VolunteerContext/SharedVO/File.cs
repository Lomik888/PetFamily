using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.SharedVO;

public class File : ValueObject
{
    public string Path { get; }

    private File(string path)
    {
        Path = path;
    }

    public static Result<File, Error> Create(string path) => new File(path);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Path;
    }
}