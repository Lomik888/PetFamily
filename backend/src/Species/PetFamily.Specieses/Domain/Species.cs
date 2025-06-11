using CSharpFunctionalExtensions;
using PetFamily.Specieses.Domain.Ids;
using PetFamily.Specieses.Domain.SharedValueObjects;

namespace PetFamily.Specieses.Domain;

public sealed class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    public Name Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;

    private Species()
    {
    }

    public Species(
        SpeciesId id,
        Name name) : base(id)
    {
        Name = name;
    }

    public void SetBreeds(List<Breed> breeds)
    {
        if (breeds.Count == 0)
            _breeds.Clear();

        var newBreeds = _breeds.Union(breeds);
        _breeds.Clear();

        _breeds.AddRange(newBreeds);
    }
}