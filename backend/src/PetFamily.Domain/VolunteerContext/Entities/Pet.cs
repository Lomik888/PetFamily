using PetFamily.Domain.Contracts;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Domain.VolunteerContext.Entities;

public sealed class Pet : SoftDeletableEntity<PetId>, ICloneable
{
    public NickName NickName { get; private set; }
    public SerialNumber SerialNumber { get; private set; }
    public SpeciesBreedId SpeciesBreedId { get; private set; }
    public Description Description { get; private set; }
    public Color Color { get; private set; }
    public HealthDescription HealthDescription { get; private set; }
    public Address Address { get; private set; }
    public Weight Weight { get; private set; }
    public Height Height { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public bool Sterilize { get; private set; }
    public DateOfBirth DateOfBirth { get; private set; }
    public bool Vaccinated { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public DetailsForHelps DetailsForHelps { get; private set; }
    public FilesPet FilesPet { get; private set; }

    private Pet(PetId id) : base(id)
    {
    }

    internal Pet(
        PetId id,
        NickName nickName,
        SerialNumber serialNumber,
        SpeciesBreedId speciesBreedId,
        Description description,
        Color color,
        HealthDescription healthDescription,
        Address address,
        Weight weight,
        Height height,
        PhoneNumber phoneNumber,
        bool sterilize,
        DateOfBirth dateOfBirth,
        bool vaccinated,
        HelpStatus helpStatus,
        CreatedAt createdAt,
        DetailsForHelps detailsForHelps,
        FilesPet filesPet) : base(id)
    {
        NickName = nickName;
        SerialNumber = serialNumber;
        SpeciesBreedId = speciesBreedId;
        Description = description;
        Color = color;
        HealthDescription = healthDescription;
        Address = address;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        Sterilize = sterilize;
        DateOfBirth = dateOfBirth;
        Vaccinated = vaccinated;
        HelpStatus = helpStatus;
        CreatedAt = createdAt;
        DetailsForHelps = detailsForHelps;
        FilesPet = filesPet;
    }

    internal void SetSerialNumber(SerialNumber serialNumber)
    {
        SerialNumber = serialNumber;
    }

    internal void SetFiles(FilesPet filesPet)
    {
        FilesPet = filesPet;
    }

    public object Clone()
    {
        return new Pet(
            PetId.Create(this.Id.Value).Value,
            NickName.Create(this.NickName.Value).Value,
            SerialNumber.Create(this.SerialNumber.Value).Value,
            SpeciesBreedId.Create(
                this.SpeciesBreedId.SpeciesId,
                this.SpeciesBreedId.BreedId).Value,
            Description.Create(this.Description.Value).Value,
            Color.Create(this.Color.Value).Value,
            HealthDescription.Create(
                this.HealthDescription.SharedHealthStatus,
                this.HealthDescription.SkinCondition,
                this.HealthDescription.MouthCondition,
                this.HealthDescription.DigestiveSystemCondition
            ).Value,
            Address.Create(
                this.Address.Country,
                this.Address.City,
                this.Address.Street,
                this.Address.HouseNumber,
                this.Address.ApartmentNumber).Value,
            Weight.Create(this.Weight.Value).Value,
            Height.Create(this.Height.Value).Value,
            PhoneNumber.Create(
                this.PhoneNumber.RegionCode,
                this.PhoneNumber.Number).Value,
            this.Sterilize,
            DateOfBirth.Create(this.DateOfBirth.Value).Value,
            this.Vaccinated,
            HelpStatus.Create(this.HelpStatus.Value).Value,
            CreatedAt.Create(this.CreatedAt.Value).Value,
            DetailsForHelps.Create(
                this.DetailsForHelps.Items
                    .Select(x => DetailsForHelp.Create(x.Title, x.Description).Value)).Value,
            FilesPet.Create(
                this.FilesPet.Items
                    .Select(x => File.Create(x.FullPath).Value)).Value
        );
    }
}