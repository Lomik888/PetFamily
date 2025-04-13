using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Domain.VolunteerContext.Entities;

public class Pet : Entity<PetId>
{
    public Name Name { get; private set; } 
    public SpeciesId SpeciesId { get; private set; } 
    public Description Description { get; private set; } 
    public BreedId BreedId { get; private set; } 
    public Color Color { get; private set; } 
    public HealthDescription HealthDescription { get; private set; } 
    public Address Address { get; private set; }
    public Weight Weight { get; private set; } 
    public Height Height { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } 
    public Sterilize Sterilize { get; private set; } 
    public DateOfBirth DateOfBirth { get; private set; } 
    public Vaccinated Vaccinated { get; private set; } 
    public HelpStatus HelpStatus { get; private set; } 
    public CreatedAt CreatedAt { get; private set; } 
    public DetailsForHelps DetailsForHelps { get; private set; }
    public Files Files { get; private set; }

    private Pet()
    {
    }

    public Pet(
        PetId id,
        Name name,
        SpeciesId speciesId,
        Description description,
        BreedId breedId,
        Color color,
        HealthDescription healthDescription,
        Address address,
        Weight weight,
        Height height,
        PhoneNumber phoneNumber,
        Sterilize sterilize,
        DateOfBirth dateOfBirth,
        Vaccinated vaccinated,
        HelpStatus helpStatus,
        CreatedAt createdAt,
        DetailsForHelps detailsForHelps,
        Files files) : base(id)
    {
        Name = name;
        SpeciesId = speciesId;
        Description = description;
        BreedId = breedId;
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
        Files = files;
    }
}