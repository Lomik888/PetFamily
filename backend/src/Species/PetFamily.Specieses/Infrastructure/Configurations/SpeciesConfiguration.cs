using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Specieses.Domain;
using PetFamily.Specieses.Domain.Ids;
using PetFamily.Specieses.Domain.SharedValueObjects;

namespace PetFamily.Specieses.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                breedId => breedId.Value,
                value => SpeciesId.Create(value).Value);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(Name.VELUE_MAX_LENGHT)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).Value);

        builder.HasMany(x => x.Breeds)
            .WithOne()
            .HasForeignKey("species_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}