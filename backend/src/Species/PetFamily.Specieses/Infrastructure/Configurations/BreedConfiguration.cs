using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Specieses.Domain;
using PetFamily.Specieses.Domain.Ids;
using PetFamily.Specieses.Domain.SharedValueObjects;

namespace PetFamily.Specieses.Infrastructure.Configurations;

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