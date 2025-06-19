using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participants_accounts", schema: "Accounts");
        builder.Property(x => x.Id).IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");

        builder.Property(x => x.FavoritePetsIds)
            .IsRequired()
            .HasColumnName("favorite_pets_ids")
            .HasColumnType("uuid[]");
    }
}