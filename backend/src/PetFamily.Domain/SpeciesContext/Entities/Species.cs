using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesContext.BreedVO;
using PetFamily.Domain.SpeciesContext.SharedVO;
using PetFamily.Domain.SpeciesContext.SpeciesVO;

namespace PetFamily.Domain.SpeciesContext.Entities;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    public Name Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public Species()
    {
    }

    public Species(
        SpeciesId id,
        Name name,
        IEnumerable<Breed> breeds) : base(id)
    {
        Name = name;
        _breeds = breeds.ToList();
    }
}