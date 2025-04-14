using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.SpeciesContext.Entities;
using PetFamily.Domain.SpeciesContext.SharedVO;
using BreedId = PetFamily.Domain.SpeciesContext.BreedVO.BreedId;

namespace PetFamily.Infrastructure.Configuration;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired()
            .HasConversion(
                breedId => breedId.Value,
                value => BreedId.Create(value).Value);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(Name.VELUE_MAX_LENGHT)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).Value);
    }
}