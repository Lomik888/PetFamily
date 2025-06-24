using PetFamily.Core.Dtos;

namespace PetFamily.Volunteers.Application.Dtos.SharedDtos;

public record DetailsForHelpCollectionDto(IReadOnlyList<DetailsForHelpDto> DetailsForHelps);