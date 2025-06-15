using CSharpFunctionalExtensions;
using PetFamily.Specieses.Domain.Ids;
using PetFamily.Specieses.Domain.SharedValueObjects;

namespace PetFamily.Specieses.Domain;

public sealed class Breed : Entity<BreedId>
{
    public Name Name { get; private set; }

    private Breed()
    {
    }

    public Breed(
        BreedId id,
        Name name) : base(id)
    {
        Name = name;
    }
}