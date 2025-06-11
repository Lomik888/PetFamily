using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Specieses.Domain.Ids;

namespace PetFamily.Specieses.Application.Queries.GetBreeds;

public class GetBreedsQueryValidator : AbstractValidator<GetBreedsQuery>
{
    public GetBreedsQueryValidator()
    {
        RuleFor(x => x.SpeciesId).MustBeValueObject(x => SpeciesId.Create(x));
        RuleFor(x => x.Page).Must(x => x > 0).WithMessageCustom("Page must be greater than 0");
        RuleFor(x => x.PageSize).Must(x => x > -1).WithMessageCustom("PageSize must be greater than -1");
    }
}