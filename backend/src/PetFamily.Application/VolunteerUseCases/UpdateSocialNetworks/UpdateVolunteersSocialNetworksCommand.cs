using PetFamily.Application.DTO.VolunteerDtos;
using PetFamily.Application.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.UpdateSocialNetworks;

public record UpdateVolunteersSocialNetworksCommand(
    Guid VolunteerId,
    SocialNetworkCollectionDto SocialNetworkCollectionDto
) : ICommand;