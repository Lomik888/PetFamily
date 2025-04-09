using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesContext.SharedVO;

namespace PetFamily.Domain.SpeciesContext.Entities;

public class Breed : Entity<Guid>
{
    public Name Name { get; private set; }

    private Breed()
    {
    }

    public Breed(Guid id, Name name) : base(id)
    {
        Name = name;
    }
}