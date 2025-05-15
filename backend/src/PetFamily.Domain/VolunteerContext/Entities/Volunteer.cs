using CSharpFunctionalExtensions;
using PetFamily.Domain.Contracts;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;
using PetFamily.Shared.Errors;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;
using Name = PetFamily.Domain.VolunteerContext.VolunteerVO.Name;

namespace PetFamily.Domain.VolunteerContext.Entities;

public sealed class Volunteer : SoftDeletableEntity<VolunteerId>, ICloneable
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

    public Result<Pet, Error> CreatePet(CreatePetDto createPetDto)
    {
        SerialNumber serialNumber;
        if (_pets.Count == 0)
        {
            serialNumber = SerialNumber.CreateFirst();
        }

        var serialNumberResult = SerialNumber.Create((uint)_pets.Count + 1);
        if (serialNumberResult.IsFailure == true)
        {
            return serialNumberResult.Error;
        }

        serialNumber = serialNumberResult.Value;
        var petId = PetId.Create().Value;
        var dateCreatedUtc = DateTime.UtcNow;

        var createdAtResult = CreatedAt.Create(dateCreatedUtc);
        if (createdAtResult.IsFailure == true)
        {
            return createdAtResult.Error;
        }

        var createdAt = createdAtResult.Value;

        var pet = new Pet(
            petId,
            createPetDto.NickName,
            serialNumber,
            createPetDto.SpeciesBreedId,
            createPetDto.Description,
            createPetDto.Color,
            createPetDto.HealthDescription,
            createPetDto.Address,
            createPetDto.Weight,
            createPetDto.Height,
            createPetDto.PhoneNumber,
            createPetDto.Sterilize,
            createPetDto.DateOfBirth,
            createPetDto.Vaccinated,
            createPetDto.HelpStatus,
            createdAt,
            createPetDto.DetailsForHelps,
            createPetDto.FilesPet);

        _pets.Add(pet);

        return pet;
    }

    public UnitResult<Error> SetPetFiles(Pet pet, FilesPet filesPets)
    {
        if (_pets.Any(p => p.Id == pet.Id) == false)
        {
            return ErrorsPreform.General.NotFound(pet.Id.Value);
        }

        pet.SetFiles(filesPets);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemovePetFiles(Pet pet, FilesPet filesPets)
    {
        if (_pets.Any(p => p.Id == pet.Id) == false)
        {
            return ErrorsPreform.General.NotFound(pet.Id.Value);
        }

        var newPetFiles = pet.FilesPet.Items.Except(filesPets.Items);
        var newFiles = FilesPet.Create(newPetFiles).Value;

        pet.SetFiles(newFiles);
        return UnitResult.Success<Error>();
    }

    public object Clone()
    {
        var volunteer = new Volunteer(
            VolunteerId.Create(this.Id.Value).Value,
            Name.Create(
                this.Name.FirstName,
                this.Name.LastName,
                this.Name.Surname).Value,
            Email.Create(this.Email.Value).Value,
            Description.Create(this.Description.Value).Value,
            Experience.Create(this.Experience.Value).Value,
            PhoneNumber.Create(
                this.PhoneNumber.RegionCode,
                this.PhoneNumber.Number).Value,
            SocialNetworks.Create(this.SocialNetworks.Items
                .Select(x => SocialNetwork.Create(x.Title, x.Url).Value)).Value,
            DetailsForHelps.Create(this.DetailsForHelps.Items
                .Select(x => DetailsForHelp.Create(x.Title, x.Description).Value)).Value,
            Files.Create(this.Files.Items
                .Select(x => File.Create(x.FullPath).Value)).Value
        );

        volunteer._pets.AddRange(this._pets.Select(x => (Pet)x.Clone()));

        return volunteer;
    }
}