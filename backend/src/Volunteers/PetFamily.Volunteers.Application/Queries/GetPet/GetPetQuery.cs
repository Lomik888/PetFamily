using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Queries.GetPet;

public record GetPetQuery(
    Guid PetId) : IQuery;