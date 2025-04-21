using PetFamily.Application.DTO.SharedDtos;
using PetFamily.Application.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.UpdateDetailsForHelps;

public record UpdateVolunteersDetailsForHelpCommand(
    Guid VolunteerId,
    DetailsForHelpCollectionDto DetailsForHelpCollection
) : ICommand;