using PetFamily.Application.Contracts.DTO.SharedDtos;
using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.UpdateDetailsForHelps;

public record UpdateVolunteersDetailsForHelpCommand(
    Guid VolunteerId,
    DetailsForHelpCollectionDto DetailsForHelpCollection
) : ICommand;