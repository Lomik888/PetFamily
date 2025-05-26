using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Queries.GetPets;

namespace PetFamily.API.Contracts.Requests;

public record GetPetsRequest(
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
    bool SortByPetDateOfBirth) : IToQuery<GetPetsQuery>
{
    public GetPetsQuery ToQuery()
    {
        return new GetPetsQuery(Page,
            PageSize,
            VolunteerId,
            NickName,
            Color,
            Age,
            BreedId,
            SpeciesId,
            Country,
            City,
            SortByPetAge,
            SortByPetWeight,
            SortByPetNickName,
            SortByPetDateOfBirth);
    }
}