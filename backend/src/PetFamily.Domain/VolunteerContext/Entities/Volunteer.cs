using CSharpFunctionalExtensions;
using PetFamily.Domain.Contracts.Abstractions;
using PetFamily.Domain.SharedVO;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;

namespace PetFamily.Domain.VolunteerContext.Entities;

public sealed class Volunteer : SoftDeletableEntity<VolunteerId>
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

    private Volunteer(VolunteerId id) : base(id)
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

    public void UpdateMainInfo(
        Name name,
        Description description,
        Experience experience)
    {
        Name = name;
        Description = description;
        Experience = experience;
    }

    public void SetSocialNetworks(SocialNetworks socialNetworks)
    {
        SocialNetworks = socialNetworks;
    }

    public void SetDetailsForHelps(DetailsForHelps detailsForHelps)
    {
        DetailsForHelps = detailsForHelps;
    }

    public override void UnActivate()
    {
        if (IsActive == false)
        {
            return;
        }

        var dateTimeUtcNow = DateTime.UtcNow;

        DeletedAt = DeletedAt.Create(dateTimeUtcNow).Value;
        IsActive = false;

        foreach (var pet in Pets)
        {
            pet.UnActivate();
        }
    }

    public override void Activate()
    {
        if (IsActive == true)
        {
            return;
        }

        DeletedAt = null;
        IsActive = true;

        foreach (var pet in Pets)
        {
            pet.Activate();
        }
    }
}