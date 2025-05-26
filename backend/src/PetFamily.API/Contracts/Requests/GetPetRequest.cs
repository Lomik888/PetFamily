using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.VolunteerUseCases.Queries.GetPet;

namespace PetFamily.API.Contracts.Requests;

public record GetPetRequest() : IToQuery<GetPetQuery, Guid>
{
    public GetPetQuery ToQuery(Guid parameter)
    {
        return new GetPetQuery(parameter);
    }
}