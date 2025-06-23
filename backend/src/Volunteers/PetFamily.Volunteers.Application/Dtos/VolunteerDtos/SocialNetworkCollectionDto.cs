using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.Dtos.VolunteerDtos;

public record SocialNetworkCollectionDto(IReadOnlyList<SocialNetworkDto> SocialNetworks);