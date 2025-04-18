using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Domain.VolunteerContext.Entities;

public sealed class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    public Experience Experience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Files Files { get; private set; }
    public SocialNetworks SocialNetworks { get; private set; }
    public DetailsForHelps DetailsForHelps { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;

    private Volunteer()
    {
    }

    public Volunteer(
        VolunteerId id,
        Name name,
        Email email,
        Description description,
        Experience experience,
        PhoneNumber phoneNumber,
        SocialNetworks socialNetworks,
        DetailsForHelps detailsForHelps,
        Files files) : base(id)
    {
        Name = name;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        SocialNetworks = socialNetworks;
        DetailsForHelps = detailsForHelps;
        Files = files;
    }

    public Result<int> FoundHomePetsCount() =>
        _pets.Count(p => p.HelpStatus.Value == HelpStatuses.FOUNDHOME);

    public Result<int> SearchingHomePetsCount() =>
        _pets.Count(p => p.HelpStatus.Value == HelpStatuses.SEARCHINGHOME);

    public Result<int> UndergoingTreatmentPetsCount() =>
        _pets.Count(p => p.HelpStatus.Value == HelpStatuses.UNDERGOINGTREATMENT);
}