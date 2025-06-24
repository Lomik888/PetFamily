using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure.ConfigurationsReadContext;

public class UserDtoConfiguration : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.ToView("users", schema: "Accounts");

        builder.Property(x => x.SocialNetworks)
            .HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                value =>
                    JsonSerializer.Deserialize<List<SocialNetworkDto>>(value, JsonSerializerOptions.Default)!)
            .HasColumnName("social_networks")
            .HasColumnType("jsonb");

        builder.Property(x => x.Photo).IsRequired().HasColumnName("photo").IsRequired(false);
        builder.Property(x => x.FullName).IsRequired().HasColumnName("full_name");

        builder.HasOne(x => x.AdminAccount)
            .WithOne()
            .HasForeignKey<AdminAccountDto>(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccountDto>(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccountDto>(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<IdentityUserRole<Guid>>(
                join =>
                    join.HasOne<RoleDto>().WithMany().HasForeignKey(x => x.RoleId),
                join =>
                    join.HasOne<UserDto>().WithMany().HasForeignKey(x => x.UserId),
                join =>
                {
                    join.HasKey(ur => new { ur.UserId, ur.RoleId });
                    join.Property(x => x.RoleId).IsRequired().HasColumnName("role_id");
                    join.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
                    join.ToView("users_roles", schema: "Accounts");
                });
    }
}