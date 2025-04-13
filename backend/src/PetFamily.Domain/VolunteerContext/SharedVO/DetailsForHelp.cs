using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.SharedVO;

public class DetailsForHelp : ValueObject
{
    private const int MIN_LENGHT = 1;
    public const int TITLE_MAX_LENGHT = 100;
    public const int DESCRIPTION_MAX_LENGHT = 500;

    public string Title { get; }
    public string Description { get; }

    private DetailsForHelp(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public static Result<DetailsForHelp, Error[]> Create(string title, string description)
    {
        var errors = new List<Error>();

        FieldValidator.ValidationField(title, MIN_LENGHT, TITLE_MAX_LENGHT, errors);
        FieldValidator.ValidationField(description, MIN_LENGHT, DESCRIPTION_MAX_LENGHT, errors);

        if (errors.Count > 0)
        {
            return errors.ToArray();
        }

        return new DetailsForHelp(title, description);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Description;
    }
}