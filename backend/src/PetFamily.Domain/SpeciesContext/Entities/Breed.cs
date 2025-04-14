﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.SpeciesContext.BreedVO;
using PetFamily.Domain.SpeciesContext.SharedVO;

namespace PetFamily.Domain.SpeciesContext.Entities;

public class Breed : Entity<BreedId>
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