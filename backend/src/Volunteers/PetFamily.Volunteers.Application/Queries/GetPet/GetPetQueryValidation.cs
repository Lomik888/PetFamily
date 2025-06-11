using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;

namespace PetFamily.Volunteers.Application.Queries.GetPet;

public class GetPetQueryValidation : AbstractValidator<GetPetQuery>
{
    public GetPetQueryValidation()
    {
        RuleFor(x => x.PetId).MustBeValueObject(x => PetId.Create(x));
    }
}