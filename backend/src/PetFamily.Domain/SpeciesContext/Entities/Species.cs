using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesContext.SharedVO;

namespace PetFamily.Domain.SpeciesContext.Entities;

public class Species : Entity<Guid>
{
    private List<Breed> _breeds = [];

    public Name Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    public Species()
    {
    }

    public Species(Guid id, Name name, IEnumerable<Breed> breeds) : base(id)
    {
        Name = name;
        _breeds = breeds.ToList();
    }
}