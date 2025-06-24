using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure.ConfigurationsReadContext;

public class RoleDtoConfiguration : IEntityTypeConfiguration<RoleDto>
{
    public void Configure(EntityTypeBuilder<RoleDto> builder)
    {
        builder.ToView("roles", schema: "Accounts");

        builder.Property(x => x.Name).IsRequired().HasColumnName("name");

        builder.HasMany(x => x.Permissions).WithMany(x => x.Roles)
            .UsingEntity<Dictionary<Guid, Guid>>(
                right => right.HasOne<PermissionDto>().WithMany().HasForeignKey("permissions_id"),
                left => left.HasOne<RoleDto>().WithMany().HasForeignKey("role_id")
            )
            .ToView("permissions_roles", schema: "Accounts");
    }
}