using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

namespace PetFamily.Volunteers.Application.Commands.UpdateSocialNetworks;

public record UpdateVolunteersSocialNetworksCommand(
    Guid VolunteerId,
    SocialNetworkCollectionDto SocialNetworkCollectionDto
) : ICommand;