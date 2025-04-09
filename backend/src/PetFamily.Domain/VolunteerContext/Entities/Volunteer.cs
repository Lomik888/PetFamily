using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using Name = PetFamily.Domain.VolunteerContext.VolunteerVO.Name;

namespace PetFamily.Domain.VolunteerContext.Entities;

public class Volunteer : Entity<Guid>
{
    private List<Pet> _pets;
    private List<SocialNetwork> _socialNetworks;
    private List<DetailsForHelp> _detailsForHelps;

    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public Description Description { get; private set; }
    public Experience Experience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<DetailsForHelp> DetailsForHelps => _detailsForHelps;

    private Volunteer()
    {
    }

    public Volunteer(
        Guid id,
        Name name,
        Email email,
        Description description,
        Experience experience,
        PhoneNumber phoneNumber,
        IEnumerable<Pet> pets,
        IEnumerable<SocialNetwork> socialNetworks,
        IEnumerable<DetailsForHelp> detailsForHelps) : base(id)
    {
        Name = name;
        Email = email;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        _pets = pets.ToList();
        _socialNetworks = socialNetworks.ToList();
        _detailsForHelps = detailsForHelps.ToList();
    }

    public Result<int> FoundHomePetsCount()
    {
        var countPetsWhoFoundHome = _pets.Count(p => p.HelpStatus.Value == HelpStatuses.FOUNDHOME);
        return countPetsWhoFoundHome;
    }

    public Result<int> SearchingHomePetsCount()
    {
        var countPetsWhoFoundHome = _pets.Count(p => p.HelpStatus.Value == HelpStatuses.SEARCHINGHOME);
        return countPetsWhoFoundHome;
    }

    public Result<int> UndergoingTreatmentPetsCount()
    {
        var countPetsWhoFoundHome = _pets.Count(p => p.HelpStatus.Value == HelpStatuses.UNDERGOINGTREATMENT);
        return countPetsWhoFoundHome;
    }
}