using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class HelpStatus : ValueObject
{
    public HelpStatuses Value { get; }

    private HelpStatus(HelpStatuses value)
    {
        Value = value;
    }

    public static Result<HelpStatus, Error> Create(HelpStatuses value)
    {
        return new HelpStatus(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}