using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.SpeciesContext.Ids;

namespace PetFamily.Application.SpeciesUseCases.Commands.Delete;

public class DeleteSpecieAndBreedCommandValidator : AbstractValidator<DeleteSpecieAndBreedCommand>
{
    public DeleteSpecieAndBreedCommandValidator()
    {
        RuleFor(x => x.SpeciesId).MustBeValueObject(x => SpeciesId.Create(x));
        RuleFor(x => x.BreedId).MustBeValueObject(x => BreedId.Create(x));
    }
}