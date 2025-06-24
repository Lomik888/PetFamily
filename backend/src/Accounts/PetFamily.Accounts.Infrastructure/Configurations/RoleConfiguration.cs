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

        builder.HasMany(x => x.Permissions).WithMany(x => x.Roles)
            .UsingEntity<Dictionary<Guid, Guid>>(
                right => right.HasOne<Permission>().WithMany().HasForeignKey("permissions_id"),
                left => left.HasOne<Role>().WithMany().HasForeignKey("role_id")
            )
            .ToTable("permissions_roles", schema: "Accounts");
    }
}