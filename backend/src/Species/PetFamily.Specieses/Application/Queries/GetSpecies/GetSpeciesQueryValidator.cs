using FluentValidation;
using PetFamily.Core.Extensions;

namespace PetFamily.Specieses.Application.Queries.GetSpecies;

public class GetSpeciesQueryValidator : AbstractValidator<GetSpeciesQuery>
{
    public GetSpeciesQueryValidator()
    {
        RuleFor(x => x.Page).Must(x => x > 0).WithMessageCustom("Page must be greater than 0");
        RuleFor(x => x.PageSize).Must(x => x > -1).WithMessageCustom("PageSize must be greater than -1");
    }
}