using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure.ConfigurationsReadContext;

public class ParticipantAccountDtoConfiguration : IEntityTypeConfiguration<ParticipantAccountDto>
{
    public void Configure(EntityTypeBuilder<ParticipantAccountDto> builder)
    {
        builder.ToView("participants_accounts", schema: "Accounts");
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");

        builder.Property(x => x.FavoritePetsIds)
            .IsRequired()
            .HasColumnName("favorite_pets_ids")
            .HasColumnType("uuid[]");
    }
}