using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.Queries.GetPet;

public record GetPetQuery(
    Guid PetId) : IQuery;