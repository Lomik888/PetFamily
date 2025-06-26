using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

public class VolunteerRequestId : ComparableValueObject
{
    public Guid Value { get; }

    private VolunteerRequestId(Guid value)
    {
        Value = value;
    }

    public static VolunteerRequestId Create()
    {
        return new VolunteerRequestId(Guid.NewGuid());
    }

    public static Result<VolunteerRequestId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            return ErrorsPreform.General.Validation("Id id invalid", nameof(VolunteerRequestId));
        }

        return new VolunteerRequestId(id);
    }

    public static VolunteerRequestId CreateEmpty()
    {
        return new VolunteerRequestId(Guid.Empty);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}