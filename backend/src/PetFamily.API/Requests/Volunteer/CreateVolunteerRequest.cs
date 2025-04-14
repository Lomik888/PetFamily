using PetFamily.Application.DTO.SharedDtos;
using PetFamily.Application.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.CreateVolunteer;

namespace PetFamily.API.Requests.Volunteer;

public record CreateVolunteerRequest(
    NameDto Name,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone,
    IEnumerable<FileDto> Files,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<DetailsForHelpDto> DetailsForHelps
)
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            Name,
            Email,
            Description,
            Experience,
            Phone,
            Files,
            SocialNetworks,
            DetailsForHelps);
    }
}