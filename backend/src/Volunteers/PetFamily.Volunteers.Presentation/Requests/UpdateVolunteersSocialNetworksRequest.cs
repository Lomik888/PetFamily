using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Commands.UpdateSocialNetworks;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

public record UpdateVolunteersSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworksDto)
    : IToCommand<UpdateVolunteersSocialNetworksCommand, Guid>
{
    public UpdateVolunteersSocialNetworksCommand ToCommand(Guid volunteerId)
    {
        return new UpdateVolunteersSocialNetworksCommand(
            volunteerId,
            new SocialNetworkCollectionDto(SocialNetworksDto.ToList()));
    }
}