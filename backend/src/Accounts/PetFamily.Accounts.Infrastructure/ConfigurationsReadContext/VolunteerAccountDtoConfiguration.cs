using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure.ConfigurationsReadContext;

public class VolunteerAccountDtoConfiguration : IEntityTypeConfiguration<VolunteerAccountDto>
{
    public void Configure(EntityTypeBuilder<VolunteerAccountDto> builder)
    {
        builder.ToView("volunteers_accounts", schema: "Accounts");

        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(x => x.Certificates).IsRequired(false).HasColumnName("certificates");

        builder.Property(x => x.Experience)
            .IsRequired()
            .HasColumnName("experience")
            .ValueGeneratedNever();

        builder.Property(x => x.DetailsForHelps)
            .HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                value => JsonSerializer.Deserialize<List<DetailsForHelpDto>>(value,
                    JsonSerializerOptions.Default)!)
            .HasColumnName("details_for_help")
            .HasColumnType("jsonb");
    }
}