using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Queries.GetPets;

namespace PetFamily.Volunteers.Presentation.Requests.Volunteer;

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