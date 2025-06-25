using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

public class VolunteerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public int Experience { get; set; }
    public SocialNetworkDto[]? SocialNetworks { get; set; }
    public int PetCount { get; set; }

    // public VolunteerDto(
    //     Guid id, 
    //     string firstName,
    //     string lastName,
    //     string surname, 
    //     int experience,
    //     IEnumerable<SocialNetworkDto>? socialNetworks, 
    //     int petCount)
    // {
    //     Id = id;
    //     FirstName = firstName;
    //     LastName = lastName;
    //     Surname = surname;
    //     Experience = experience;
    //     SocialNetworks = socialNetworks?.ToArray();
    //     PetCount = petCount;
    // }
}