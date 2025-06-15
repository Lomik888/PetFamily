using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Volunteers.Application.Dtos.SharedDtos;

namespace PetFamily.Volunteers.Application.Commands.UpdateDetailsForHelps;

public record UpdateVolunteersDetailsForHelpCommand(
    Guid VolunteerId,
    DetailsForHelpCollectionDto DetailsForHelpCollection
) : ICommand;