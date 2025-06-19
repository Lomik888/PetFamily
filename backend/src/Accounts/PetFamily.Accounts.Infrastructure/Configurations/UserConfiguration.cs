using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.ValueObjects;
using PetFemily.Accounts.Domain;
using PetFemily.Accounts.Domain.ValueObjects;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", schema: "Accounts");

        builder.OwnsOne(x => x.SocialNetworks, xb =>
        {
            xb.Property(x => x.Items)
                .HasConversion(
                    value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                    value => JsonSerializer.Deserialize<IReadOnlyList<SocialNetwork>>(value,
                        JsonSerializerOptions.Default)!,
                    new ValueComparer<IReadOnlyList<SocialNetwork>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (current, value) => HashCode.Combine(current, value.GetHashCode())),
                        c => c.ToList()
                    ))
                .HasColumnName("social_networks")
                .HasColumnType("jsonb");
        });

        builder.Property(x => x.Photo).IsRequired().HasColumnName("username")
            .HasConversion(
                file => file != null ? file.FullPath : null,
                value => value == null ? null : File.Create(value).Value)
            .IsRequired(false);

        builder.Property(x => x.FullName).IsRequired().HasColumnName("full_name");

        builder.Property(x => x.RoleId).IsRequired().HasColumnName("role_id");

        builder.HasOne(x => x.AdminAccount)
            .WithOne()
            .HasForeignKey<AdminAccount>(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccount>(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccount>(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}