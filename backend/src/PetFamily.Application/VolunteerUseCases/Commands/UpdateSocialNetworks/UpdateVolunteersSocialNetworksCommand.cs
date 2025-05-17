using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Commands.UpdateSocialNetworks;

public record UpdateVolunteersSocialNetworksCommand(
    Guid VolunteerId,
    SocialNetworkCollectionDto SocialNetworkCollectionDto
) : ICommand;