using PetFamily.Application.Contracts.DTO.VolunteerDtos;

namespace PetFamily.Application.Contracts.DTO;

public class VolunteerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public int Experience { get; set; }
    public SocialNetworkDto[]? SocialNetworks { get; set; }
    public int PetCount { get; set; }
}