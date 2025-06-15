using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Volunteers.Domain.ValueObjects.SharedVO;

public class DetailsForHelp : ValueObject
{
    private const int MIN_LENGHT = 1;
    public const int TITLE_MAX_LENGHT = 100;
    public const int DESCRIPTION_MAX_LENGHT = 500;

    public string Title { get; }
    public string Description { get; }

    [JsonConstructor]
    private DetailsForHelp(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public static Result<DetailsForHelp, ErrorList> Create(string title, string description)
    {
        var errors = new List<Error>();

        Validator.FieldValueObject.Validation(title, MIN_LENGHT, TITLE_MAX_LENGHT, errors);
        Validator.FieldValueObject.Validation(description, MIN_LENGHT, DESCRIPTION_MAX_LENGHT, errors);

        if (errors.Count > 0)
        {
            return ErrorList.Create(errors);
        }

        return new DetailsForHelp(title, description);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}