using PetFamily.Domain.Contracts.Abstractions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Collections;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.SharedVO.Collections;

namespace PetFamily.Domain.VolunteerContext.Entities;

public sealed class Pet : SoftDeletableEntity<PetId>
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

    public Pet(
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

    public void SetSerialNumber(SerialNumber serialNumber)
    {
        SerialNumber = serialNumber;
    }
}