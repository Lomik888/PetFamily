using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Domain.ValueObjects;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFemily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    public SocialNetworks SocialNetworks { get; set; } = SocialNetworks.CreateEmpty().Value;
    public Guid RoleId { get; set; }
    public File? Photo { get; set; }
    public string FullName { get; set; } = default!;
    public AdminAccount? AdminAccount { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
}