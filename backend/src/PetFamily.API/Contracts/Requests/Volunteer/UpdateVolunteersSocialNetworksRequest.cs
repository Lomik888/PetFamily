﻿using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.VolunteerUseCases.Commands.UpdateSocialNetworks;

namespace PetFamily.API.Contracts.Requests.Volunteer;

public record UpdateVolunteersSocialNetworksRequest(
    IEnumerable<SocialNetworkDto> SocialNetworksDto
)
    : IToCommand<UpdateVolunteersSocialNetworksCommand, Guid>
{
    public UpdateVolunteersSocialNetworksCommand ToCommand(Guid volunteerId)
    {
        return new UpdateVolunteersSocialNetworksCommand(
            volunteerId,
            new SocialNetworkCollectionDto(SocialNetworksDto.ToList()));
    }
}