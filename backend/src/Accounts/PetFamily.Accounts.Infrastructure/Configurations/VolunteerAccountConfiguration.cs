using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Domain;
using PetFemily.Accounts.Domain.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteers_accounts", schema: "Accounts");
        builder.Property(x => x.Id).IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");

        builder.Property(x => x.Certificates).IsRequired(false).HasColumnName("certificates");

        builder.Property(x => x.Experience)
            .IsRequired()
            .HasColumnName("experience")
            .ValueGeneratedNever()
            .HasConversion(
                experience => experience.Value,
                value => Experience.Create(value).Value);

        builder.OwnsOne(x => x.DetailsForHelps, xb =>
        {
            xb.Property(x => x.Items)
                .HasConversion(
                    value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                    value => JsonSerializer.Deserialize<IReadOnlyList<DetailsForHelp>>(value,
                        JsonSerializerOptions.Default)!,
                    new ValueComparer<IReadOnlyList<DetailsForHelp>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (current, value) => HashCode.Combine(current, value.GetHashCode())),
                        c => c.ToList()
                    )
                )
                .HasColumnName("details_for_help")
                .HasColumnType("jsonb");
        });
    }
}