using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Domain.ValueObjects;

namespace PetFemily.Accounts.Domain;

public class VolunteerAccount
{
    public const string RoleName = "Volunteer";
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Certificates { get; set; }
    public DetailsForHelps DetailsForHelps { get; set; } = DetailsForHelps.CreateEmpty().Value;
    public Experience Experience { get; set; } = Experience.Create(0).Value;
}