using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        builder.Property(x => x.Photo).IsRequired().HasColumnName("photo")
            .HasConversion(
                file => file != null ? file.FullPath : null,
                value => value == null ? null : File.Create(value).Value)
            .IsRequired(false);

        builder.Property(x => x.FullName).IsRequired().HasColumnName("full_name");

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

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<IdentityUserRole<Guid>>(
                join =>
                    join.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
                join =>
                    join.HasOne<User>().WithMany().HasForeignKey(x => x.UserId),
                join =>
                {
                    join.HasKey(ur => new { ur.UserId, ur.RoleId });
                    join.Property(x => x.RoleId).IsRequired().HasColumnName("role_id");
                    join.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
                    join.ToTable("users_roles", schema: "Accounts");
                });
    }
}