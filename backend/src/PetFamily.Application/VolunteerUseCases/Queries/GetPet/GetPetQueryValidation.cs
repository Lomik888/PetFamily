using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;

namespace PetFamily.Application.VolunteerUseCases.Queries.GetPet;

public class GetPetQueryValidation : AbstractValidator<GetPetQuery>
{
    public GetPetQueryValidation()
    {
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
    }
}