using CSharpFunctionalExtensions;
using PetFamily.Domain.Contracts.Abstractions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;
using PetFamily.Shared.Errors;
using Name = PetFamily.Domain.VolunteerContext.VolunteerVO.Name;

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

    // Пока заглушка для UnitTests
    public void AddPets(IEnumerable<Pet> pets)
    {
        _pets.AddRange(pets);
    }

    // Пока заглушка для UnitTests
    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
    }

    public override void UnActivate()
    {
        if (IsActive == false)
        {
            return;
        }

        var dateTimeUtcNow = DateTime.UtcNow;

        DeletedAt = dateTimeUtcNow;
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

    public UnitResult<Error> MovePet(Pet pet, SerialNumber serialNumber)
    {
        var petsCount = _pets.Count;
        var petSerialNumber = pet.SerialNumber.Value;
        var petId = pet.Id;

        switch (true)
        {
            case var _ when petsCount == 0:
            {
                return ErrorsPreform.General.Validation("No pets were found", nameof(Pets));
            }
            
            case var _ when _pets.Any(p => p.Id == petId) == false:
            {
                return ErrorsPreform.General.Validation(
                    $"The animal does not belong to the volunteer, " +
                    $"no contains {petId.Value} in volunteer's id:{this.Id.Value} pets",
                    nameof(Pets));
            }
            
            case var _ when serialNumber.Value == pet.SerialNumber.Value:
            {
                return UnitResult.Success<Error>();
            }
            
            case var _ when serialNumber.Value > petsCount:
            {
                foreach (var p in _pets.Where(x =>
                             x.SerialNumber.Value > petSerialNumber))
                {
                    p.SetSerialNumber(p.SerialNumber.CurrentValueMinusOne());
                }

                var newPetSerialNumberResult = SerialNumber.Create((uint)petsCount);
                if (newPetSerialNumberResult.IsFailure)
                {
                    return newPetSerialNumberResult.Error;
                }

                serialNumber = newPetSerialNumberResult.Value;
                break;
            }

            case var _ when serialNumber.Value < petSerialNumber:
            {
                foreach (var p in _pets.Where(x =>
                             x.SerialNumber.Value < petSerialNumber &&
                             x.SerialNumber.Value >= serialNumber.Value))
                {
                    p.SetSerialNumber(p.SerialNumber.CurrentValuePlusOne());
                }

                break;
            }

            case var _ when serialNumber.Value > petSerialNumber:
            {
                foreach (var p in _pets.Where(x =>
                             x.SerialNumber.Value > petSerialNumber &&
                             x.SerialNumber.Value <= serialNumber.Value))
                {
                    p.SetSerialNumber(p.SerialNumber.CurrentValueMinusOne());
                }

                break;
            }
        }

        pet.SetSerialNumber(serialNumber);
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> MovePetToFirst(Pet pet)
    {
        var serialNumber = SerialNumber.CreateFirst();

        var result = MovePet(pet, serialNumber);
        if (result.IsFailure)
        {
            return result.Error;
        }

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> MovePetToLast(Pet pet)
    {
        var petsCount = _pets.Count;

        var serialNumberResult = SerialNumber.Create((uint)petsCount);
        if (serialNumberResult.IsFailure)
        {
            return serialNumberResult.Error;
        }

        var result = MovePet(pet, serialNumberResult.Value);
        if (result.IsFailure)
        {
            return result.Error;
        }

        return UnitResult.Success<Error>();
    }
}