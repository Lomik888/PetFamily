using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesContext.Ids;
using PetFamily.Domain.SpeciesContext.SharedVO;

namespace PetFamily.Domain.SpeciesContext.Entities;

public sealed class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    public Name Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public Species()
    {
    }

    public Species(
        SpeciesId id,
        Name name) : base(id)
    {
        Name = name;
    }
}