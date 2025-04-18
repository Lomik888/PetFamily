using PetFamily.Application.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.CreateVolunteer;

namespace PetFamily.API.Requests.Volunteer;

public record CreateVolunteerRequest(
    NameDto Name,
    string Email,
    string Description,
    int Experience,
    PhoneNumberDto Phone
)
{
    public CreateVolunteerCommand ToCommand()
    {
        return new CreateVolunteerCommand(
            Name,
            Email,
            Description,
            Experience,
            Phone);
    }
}