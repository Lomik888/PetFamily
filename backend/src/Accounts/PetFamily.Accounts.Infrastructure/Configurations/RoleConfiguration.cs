using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles", schema: "Accounts");

        builder.Property(x => x.Name).IsRequired().HasColumnName("name");

        builder.HasMany(x => x.Permissions).WithMany(x => x.Roles);

        builder
            .HasMany(x => x.Users)
            .WithOne()
            .HasForeignKey(x => x.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}