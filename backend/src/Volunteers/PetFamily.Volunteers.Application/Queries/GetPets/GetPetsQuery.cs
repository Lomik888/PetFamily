using PetFamily.Core.Abstrations.Interfaces;

namespace PetFamily.Volunteers.Application.Queries.GetPets;

public record GetPetsQuery(
    int Page,
    int PageSize,
    Guid? VolunteerId,
    string? NickName,
    string? Color,
    uint? Age,
    Guid? BreedId,
    Guid? SpeciesId,
    string? Country,
    string? City,
    SortByPetAge? SortByPetAge,
    bool SortByPetWeight,
    bool SortByPetNickName,
    bool SortByPetDateOfBirth) : IQuery;

public record SortByPetAge(bool Sort, bool Asc);