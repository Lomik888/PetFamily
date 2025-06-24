using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Domain.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain;

public class VolunteerInfo : ValueObject
{
    public string? Certificates { get; }
    public Experience Experience { get; }
    public DetailsForHelps DetailsForHelps { get; }

    public VolunteerInfo(
        string? certificates,
        Experience experience,
        DetailsForHelps detailsForHelps)
    {
        Certificates = certificates;
        Experience = experience;
        DetailsForHelps = detailsForHelps;
    }

    public Result<VolunteerInfo, Error> Create(
        string? certificates,
        Experience experience,
        DetailsForHelps detailsForHelps)
    {
        if (experience == null || detailsForHelps == null)
        {
            var error = ErrorsPreform.General.Validation("Error creating volunteer info");
        }

        return new VolunteerInfo(certificates, experience!, detailsForHelps!);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        if (Certificates != null) yield return Certificates;
        yield return Experience.Value;
        foreach (var item in DetailsForHelps.Items)
        {
            yield return item;
        }
    }
}