using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Domain.ValueObjects;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFemily.Accounts.Domain;

public sealed class User : IdentityUser<Guid>
{
    public const string ROLE_NAME = "User";
    public SocialNetworks SocialNetworks { get; set; } = SocialNetworks.CreateEmpty().Value;
    public File? Photo { get; set; }
    public string FullName { get; set; } = default!;
    public AdminAccount? AdminAccount { get; set; }
    public VolunteerAccount? VolunteerAccount { get; set; }
    public ParticipantAccount? ParticipantAccount { get; set; }
    public List<Role> Roles { get; set; }

    private User()
    {
    }

    public User(
        string userName,
        string email,
        string fullName)
    {
        UserName = userName;
        Email = email;
        FullName = fullName;
    }

    public static User CreateAdmin(
        string userName,
        string email,
        string fullName)
    {
        var user = new User(
            userName,
            email,
            fullName);

        var admin = new AdminAccount();

        user.AdminAccount = admin;

        return user;
    }

    public static User CreateParticipantAccount(
        string userName,
        string email,
        string fullName)
    {
        var user = new User(
            userName,
            email,
            fullName);

        var participant = new ParticipantAccount();

        user.ParticipantAccount = participant;

        return user;
    }
}