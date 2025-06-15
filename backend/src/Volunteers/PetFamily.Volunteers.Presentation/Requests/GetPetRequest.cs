using PetFamily.Framework.Abstractions;
using PetFamily.Volunteers.Application.Queries.GetPet;

namespace PetFamily.Volunteers.Presentation.Requests;

public record GetPetRequest() : IToQuery<GetPetQuery, Guid>
{
    public GetPetQuery ToQuery(Guid parameter)
    {
        return new GetPetQuery(parameter);
    }
}