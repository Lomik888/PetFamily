using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public class SocialNetwork : ValueObject
{
    private const int MIN_LENGTH = 1;
    public const int TITLE_MAX_LENGTH = 20;
    public const int URL_MAX_LENGTH = 100;

    private static readonly Regex UrlRegex = new Regex(
        REGULAR_URL,
        RegexOptions.Compiled |
        RegexOptions.IgnoreCase |
        RegexOptions.IgnorePatternWhitespace);

    private const string REGULAR_URL =
        """
        ^(https?:\/\/)?(www\.)?
        (facebook\.com|twitter\.com|instagram\.com|linkedin\.com|vk\.com|youtube\.com|tiktok\.com)
        \/[A-Za-z0-9_.-]+(?:\/[^\s]*)?$
        """;

    public string Title { get; }
    public string Url { get; }

    [JsonConstructor]
    private SocialNetwork(string title, string url)
    {
        Url = url;
        Title = title;
    }

    public static Result<SocialNetwork, ErrorList> Create(string title, string url)
    {
        var errors = new List<Error>();

        Validator.FieldValueObject.Validation(title, MIN_LENGTH, TITLE_MAX_LENGTH, errors);
        Validator.FieldValueObject.Validation(url, MIN_LENGTH, URL_MAX_LENGTH, errors);

        if (errors.Count > 0)
        {
            return ErrorList.Create(errors);
        }

        if (UrlRegex.IsMatch(url) == false)
        {
            errors.Add(ErrorsPreform.General.Validation("Social network Url is invalid", nameof(SocialNetwork)));
            return ErrorList.Create(errors);
        }

        return new SocialNetwork(title, url);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Url;
    }
}