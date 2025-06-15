using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.Specieses.Domain.Ids;

namespace PetFamily.Specieses.Application.Commands.Delete;

public class DeleteSpecieAndBreedCommandValidator : AbstractValidator<DeleteSpecieAndBreedCommand>
{
    public DeleteSpecieAndBreedCommandValidator()
    {
        RuleFor(x => x.SpeciesId).MustBeValueObject(x => SpeciesId.Create(x));
        RuleFor(x => x.BreedId).MustBeValueObject(x => BreedId.Create(x));
    }
}